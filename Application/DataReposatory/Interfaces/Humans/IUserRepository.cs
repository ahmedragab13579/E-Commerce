using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces.Humans
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> IsUserNameExist(string UserName);
        Task<User> GetUserByEmail(string Email);
        Task<List<User>> GetMostActiveCustomersAsync(int count = 10);

        Task<bool> IsEmailExist(string Email);

        Task<User?> GetByUserNameAsync(string UserName);

        Task<bool>IsRoleExit(int Id);

        Task<List<User>> GetAllActiveUsers();


        Task<User> GetUserWithOrders(int id);
       

    }
}
