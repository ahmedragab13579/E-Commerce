using Application.DataReposatory.Interfaces;
using Application.DataReposatory.Interfaces.Carts;
using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.CartItem;
using Application.Dtos.Carts;
using Application.Results;
using Application.Services.InterFaces.CartSession;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.Mapping;
using E_Domain.Models;
using E_Infrastructure.Services.Implementaions.Mapping;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.Services.Implementaions.CardSession
{
    public class CartSessionService: ICartSessionService
    {
        private const string SessionKeyPrefix = "Cart_";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMappingServices _mappingServices;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;


        private CartDto? _cart;
        private int? _loadedUserId;
        public CartSessionService(IUnitOfWork _unitOfWork,ICurrentUserService _currentUserService,
            IHttpContextAccessor httpContextAccessor, 
            IMappingServices mappingServices)
        {
            _httpContextAccessor = httpContextAccessor;
            this._unitOfWork = _unitOfWork;
            _mappingServices = mappingServices;
            this._currentUserService = _currentUserService;
        }
     
        private ISession Session => _httpContextAccessor.HttpContext!.Session;
        private string SessionKey(int UserId) => $"{SessionKeyPrefix}{UserId}";
        private async Task<Result<CartDto>> LoadCart()
        {
            int? UserId = _currentUserService.GetCurrentUserId();
            if ( UserId==null)
            {
                return Result<CartDto>.Fail("UNAUTHORIZED", "User is not authenticated.");
            }

            if (_cart != null) return Result<CartDto>.Ok(_cart);

            if (_loadedUserId == null)
            {
                _loadedUserId = UserId;
            }

            var cart = Session.GetObject<CartDto>(SessionKey(_loadedUserId.Value));

            if (cart == null) 
            {
                Cart? dbCart = await _unitOfWork.Cart.GetCartByUserIdAsync(_loadedUserId.Value);
                if (dbCart == null)
                {
                    cart = new CartDto { UserId = _loadedUserId.Value, CartItems = new List<CartItemDto>() };
                }
                else
                {
                    cart = _mappingServices.Map<Cart, CartDto>(dbCart);
                }
                Session.SetObject(SessionKey(_loadedUserId.Value), cart);
            }

            _cart = cart;
            return Result<CartDto>.Ok(_cart);
        }




        private void SaveCartToSession(CartDto cartDto)
        {
            if (cartDto == null || _loadedUserId == null) return;
            Session.SetObject(SessionKey(_loadedUserId.Value), cartDto);
            _cart = cartDto; 
        }


        public async Task<Result<bool>> AddItemAsync(AddCartItemDto item)
        {
            var cartResult = await LoadCart();
            if (!cartResult.Success)
            {
                return Result<bool>.Fail(cartResult.Code, cartResult.Message);
            }

            var cartDto = cartResult.Data;
            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                return Result<bool>.Fail("NOT_FOUND", "Product Not Found");
            }

            try
            {
                var existingItem = cartDto.CartItems.FirstOrDefault(i => i.ProductId == item.ProductId);
                int newQuantity=0;

                if (existingItem != null)
                {
                    newQuantity = existingItem.Quantity + item.Quantity;
                }
                else
                {
                    newQuantity = item.Quantity;
                }

               
                var validationItem = new CartItem(cartDto.Id, product, newQuantity);

                if (existingItem == null)
                {
                    cartDto.CartItems.Add(_mappingServices.Map<CartItem, CartItemDto>(validationItem));
                }
                else
                {
                    existingItem.Quantity = newQuantity;
                }

                SaveCartToSession(cartDto);

                return Result<bool>.Ok(true, "Item added to session cart");
            }
            catch (ArgumentOutOfRangeException ex) 
            {
                return Result<bool>.Fail("INVALID_QTY", ex.Message);
            }
            catch (InvalidOperationException ex) 
            {
                return Result<bool>.Fail("LESS_AMOUNT", ex.Message);
            }
        }


        public async Task<Result<bool>> UpdateItemAsync(UpdateCartItemDto item)
        {
            var cart = await LoadCart();
            if (!cart.Success)
            {
                return Result<bool>.Fail(cart.Code, cart.Message);
            }



            var existing = cart.Data.CartItems.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existing == null) return Result<bool>.Fail("NOT_FOUND", "Item Not Found");
            var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                return Result<bool>.Fail("NOT_FOUND", "Product Not Found");
            }
            try
            {
                var validationItem = new CartItem(existing.Id, product, item.Quantity);
                existing.Quantity = item.Quantity;
                SaveCartToSession(cart.Data);
                return Result<bool>.Ok(true, "Item updated in session cart");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Result<bool>.Fail("INVALID_QTY", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Result<bool>.Fail("LESS_AMOUNT", ex.Message);

            }



        }

        public async Task<Result<bool>> RemoveItemAsync(int productId)
        {
            var cart =await LoadCart();
            if (!cart.Success)
            {
                return Result<bool>.Fail(cart.Code, cart.Message);
            }
            var item = cart.Data.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return Result<bool>.Fail("NOT_FOUND", "Item Not Found");

            cart.Data.CartItems.Remove(item);
            SaveCartToSession(cart.Data);
            return Result<bool>.Ok(true, "Item removed");

        }

        public async Task PersistToDbAsync() 
        {
            var cart = await LoadCart();
            if (cart.Success)
            {
                var domainCart = _mappingServices.Map<CartDto, Cart>(cart.Data);

                var existing = await _unitOfWork.Cart.GetCartByUserIdAsync(_loadedUserId.Value);
                if (existing == null)
                {
                    await _unitOfWork.Cart.AddAsync(domainCart);
                }
                else
                {
                    existing.SyncItems(domainCart.CartItems);
                     _unitOfWork.Cart.Update(existing);
                }
            }

        }

        public async Task<Result<bool>> ClearSessionCartAsync()
        {
            var cartResult = await LoadCart(); 
            if (cartResult.Success)
            {
                if (_loadedUserId == null)
                {
                    _loadedUserId = _currentUserService.GetCurrentUserId();
                }

                var newCartDto = new CartDto { UserId = _loadedUserId.Value, CartItems = new List<CartItemDto>() };

                SaveCartToSession(newCartDto);
                return Result<bool>.Ok(true, "Session cart cleared");
            }
            else
            {
                return Result<bool>.Fail(cartResult.Code, cartResult.Message);
            }
        }
        public async  Task< Result<CartDto>> GetCartAsync()
        {
            var cart = await LoadCart();
            return cart;
           
        }

    }
}
