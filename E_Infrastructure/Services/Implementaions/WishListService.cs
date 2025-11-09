using Application.DataReposatory.Interfaces;
using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.Product;
using Application.Dtos.Wishlist;
using Application.Results;
using Application.Services;
using Application.Services.InterFaces;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.Mapping;
using E_Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;


namespace E_Infrastructure.Services.Implementaions
{
    public class WishListService : IWishListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMappingServices _mappingServices; 

        public WishListService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMappingServices mappingServices )
        { 
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mappingServices = mappingServices;
        }

        public async Task<Result<bool>> AddToWishList(int productId)
        {
            try
            {
                int? currentUserId = _currentUserService.GetCurrentUserId();
                if (currentUserId == null)
                {
                    return Result<bool>.Fail("UNAUTHORIZED", "User is not authenticated.");
                }

                var wishList =  await _unitOfWork.wishListRepository.GetOrCreateWishlistAsync(currentUserId.Value);
                if (wishList == null)
                    return Result<bool>.Fail("NOT_FOUND", "WishList is not found.");
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null|| product.IsDeleted.Value)
                    return Result<bool>.Fail("NOT_FOUND", "Product is not found.");
                wishList.AddItem(product);
                _unitOfWork.wishListRepository.Update(wishList);
                int success = await _unitOfWork.CompleteTask();

                if (success > 0)
                {
                    return Result<bool>.Ok(true, "Product added to wishlist successfully.");
                }
                return Result<bool>.Fail("SAVE_FAILED", "Could not add product to wishlist.");
            }
            catch (KeyNotFoundException ex) 
            {
                return Result<bool>.Fail("PRODUCT_NOT_FOUND", ex.Message);
            }
            catch (InvalidOperationException ex) 
            {
                return Result<bool>.Fail("ALREADY_EXISTS_OR_INVALID", ex.Message);
            }
            catch(ArgumentNullException ex)
            {
                return Result<bool>.Fail("NULL_ARGUMENT", ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Result<bool>.Fail("INVALID_ARGUMENT", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<WishListDto>> GetWishListByUserId()
        {
            int? currentUserId = _currentUserService.GetCurrentUserId();
           if (currentUserId == null)
            {
                return Result<WishListDto>.Fail("UNAUTHORIZED", "User is not authenticated.");
            }
            try
            {
                

                var wishlist = await _unitOfWork.wishListRepository.GetOrCreateWishlistAsync(currentUserId.Value    );
                if (wishlist == null)
                {
                    return Result<WishListDto>.Ok(new WishListDto { UserId = currentUserId.Value }, "Wishlist not found.");
                }

                return Result<WishListDto>.Ok( _mappingServices.Map<Wishlist,WishListDto>(wishlist) , "Wishlist retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Result<WishListDto>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }


        public async Task<Result<bool>> ClearWishList()
        {
            try
            {
                int?  currentUserId = _currentUserService.GetCurrentUserId();
                if (currentUserId == null)
                {
                    return Result<bool>.Fail("UNAUTHORIZED", "User is not authenticated.");
                }

                var wishList = await _unitOfWork.wishListRepository.GetOrCreateWishlistAsync(currentUserId.Value);
                if (wishList == null || !wishList.WishlistItems.Any()) 
                {
                    return Result<bool>.Ok(true, "Wishlist is already empty."); 
                }

                _unitOfWork.wishListItemRepository.RemoveRange(wishList.WishlistItems); 
                                                                               
                                                                               

                int success = await _unitOfWork.CompleteTask();
                return Result<bool>.Ok(success >= 0, "Wishlist cleared successfully."); 
            }
            catch (Exception ex) 
            { 
                return Result<bool>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }
   
        public async Task<Result<bool>> RemoveFromWishList(int productId)
        {
            try
            {
                int? currentUserId = _currentUserService.GetCurrentUserId();

                if (currentUserId == null)
                {
                    return Result<bool>.Fail("UNAUTHORIZED", "User is not authenticated.");
                }

                var wishList = await _unitOfWork.wishListRepository.GetOrCreateWishlistAsync(currentUserId.Value);
                if (wishList == null)
                {
                    return Result<bool>.Fail("NOT_FOUND", "Wishlist not found.");
                }

                var product= await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                {
                    return Result<bool>.Fail("NOT_FOUND", "Product not found.");
                }
                if(product.IsDeleted.Value)
                {
                    return Result<bool>.Fail("INVALID_PRODUCT", "Cannot remove a deleted product from the wishlist.");
                }

                wishList.RemoveItem(productId);
                 _unitOfWork.wishListRepository.Update(wishList);
                int success = await _unitOfWork.CompleteTask();

                if (success > 0)
                {
                    return Result<bool>.Ok(true, "Product removed from wishlist successfully.");
                }
               
                return Result<bool>.Fail("NOT_FOUND_OR_FAILED", "Could not remove product from wishlist (it might not have been there).");
            }
            catch (InvalidOperationException ex) 
            {
                return Result<bool>.Fail("REMOVE_FAILED", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }


        public async Task<Result<bool>> DeleteWishList(int wishListId)
        {
            try
            {
                int? currentUserId = _currentUserService.GetCurrentUserId();
                if (currentUserId == null)
                {
                    return Result<bool>.Fail("UNAUTHORIZED", "User is not authenticated.");
                }

                var wishlist = await _unitOfWork.wishListRepository.GetByIdAsync(wishListId); 
                if(wishlist.WishlistItems != null && wishlist.WishlistItems.Any())
                    return Result<bool>.Fail("NOT_EMPTY", "Cannot delete a wishlist that contains items. Please clear the wishlist first.");

                if (wishlist == null)
                {
                    return Result<bool>.Fail("NOT_FOUND", "Wishlist not found.");
                }

                if (wishlist.UserId != currentUserId )
                {
                    return Result<bool>.Fail("FORBIDDEN", "You do not have permission to delete this wishlist.");
                }

               await _unitOfWork.wishListRepository.Delete(wishListId); 
                int success = await _unitOfWork.CompleteTask();

                if (success > 0)
                {
                    return Result<bool>.Ok(true, "Wishlist deleted successfully.");
                }
                return Result<bool>.Fail("DELETE_FAILED", "Could not delete the wishlist.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

      
  
    }
}