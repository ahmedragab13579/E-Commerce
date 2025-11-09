using Application.DataReposatory.Interfaces.Carts;
using Application.DataReposatory.Interfaces.Dashboard;
using Application.DataReposatory.Interfaces.Humans;
using Application.DataReposatory.Interfaces.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        ICartRepository Cart { get; }

        IDashBoardRepository DashBoard { get; }
        IUserRepository Users { get; }
        IOrderRepository Orders { get; }
        ICategoryRepository Category { get; }
        IPaymentRepository Payment { get; }
        IRoleRepository Role { get; }
        IProductRepository Products { get; }

        IReviewRepository reviewRepository { get; }

        IWishListRepository wishListRepository { get; }

        IWishListItemRepository wishListItemRepository { get; }


        Task<int> CompleteTask();
    }
}
