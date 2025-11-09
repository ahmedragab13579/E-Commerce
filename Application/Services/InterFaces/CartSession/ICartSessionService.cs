using Application.DataReposatory.Interfaces.Carts;
using Application.Dtos.CartItem;
using Application.Dtos.Carts;
using Application.Results;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces.CartSession
{
    public interface ICartSessionService
    {


        Task<Result<CartDto>> GetCartAsync();
        Task<Result<bool>> AddItemAsync( AddCartItemDto item);
        Task<Result<bool>> UpdateItemAsync( UpdateCartItemDto item);
        Task<Result<bool>> RemoveItemAsync( int productId);
        Task PersistToDbAsync( );
        Task<Result<bool>> ClearSessionCartAsync();
    }
}
