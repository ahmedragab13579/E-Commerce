using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.Wishlist;
using Application.Results;
using Application.Services.InterFaces.Humans;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces
{
    public interface IWishListService
    {
        Task<Result<bool>> AddToWishList(int productId);



        Task<Result<WishListDto>> GetWishListByUserId();



        Task<Result<bool>> RemoveFromWishList(int productId);


        Task<Result<bool>> ClearWishList();


        Task<Result<bool>> DeleteWishList(int wishListId);
       
    }
}
