using Application.DataReposatory.Interfaces;
using Application.DataReposatory.Interfaces.Carts;
using Application.DataReposatory.Interfaces.Dashboard;
using Application.DataReposatory.Interfaces.Humans;
using Application.DataReposatory.Interfaces.Orders;
using Application.DataReposatory.Interfaces.UnitOfWork;
using E_Infrastructure.Data;
using E_Infrastructure.DataRepository.Implementaion.Carts;
using E_Infrastructure.DataRepository.Implementaion.Dashboard;
using E_Infrastructure.DataRepository.Implementaion.Humans;
using E_Infrastructure.DataRepository.Implementaion.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly E_ApplicationDbContext _context;
    public ICartRepository Cart { get; private set; }
    public IDashBoardRepository DashBoard { get; private set; }
    public IUserRepository Users { get; private set; }
    public IOrderRepository Orders { get; private set; }
    public ICategoryRepository Category { get; private set; }
    public IPaymentRepository Payment { get; private set; }
    public IRoleRepository Role { get; private set; }
    public IProductRepository Products { get; private set; }

        public IReviewRepository reviewRepository { get; private set; }


        public IWishListRepository wishListRepository { get; private set; }


        public IWishListItemRepository wishListItemRepository { get; private set; }

        public  UnitOfWork(E_ApplicationDbContext _context)
        {
            this._context = _context;
            Cart = new CartRepository(_context);
            DashBoard = new DashBoardRepository(_context);
                
            Users = new UserRepository(_context);
            Orders = new OrderRepository(_context);
            Category = new CategoryRepository(_context);
            Payment = new PaymentRepository(_context);
            Role = new RoleRepository(_context);
            Products = new ProductRepository(_context);
            reviewRepository=new ReviewRepository(_context);
            wishListRepository = new WishListRepository(_context);
            wishListItemRepository = new WishListItemRepository(_context);



        }
        public async Task<int> CompleteTask()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
