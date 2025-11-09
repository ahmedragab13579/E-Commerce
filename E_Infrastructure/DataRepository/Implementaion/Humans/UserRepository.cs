using Application.DataReposatory.Interfaces.Humans;
using Application.Dtos.User;
using Azure.Core.GeoJson;
using E_Domain.Models;
using E_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion.Humans
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(E_ApplicationDbContext _context): base(_context)
        { 
            
        }

        public async Task<User?> GetByUserNameAsync(string UserName)
        {
            return await _context.Users.Include(n => n.Role).FirstOrDefaultAsync(N => N.UserName == UserName.Trim());
        }
        public Task<bool> IsEmailExist(string Email)
        {
            return _context.Users.AnyAsync(N => N.Email == Email.Trim());
        }

        public Task<List<User>> GetAllActiveUsers()
        {
            return _context.Users.Where(n => ! n.IsBlocked).ToListAsync();
        }

        public async Task<List<User>> GetMostActiveCustomersAsync(int count = 10)
        {
          
            var topCustomerIds = await _context.Orders
                .GroupBy(o => o.UserId) 
                .Select(g => new {
                    CustomerId = g.Key,
                    OrderCount = g.Count() 
                })
                .OrderByDescending(x => x.OrderCount) 
                .Take(count) 
                .Select(x => x.CustomerId) 
                .ToListAsync();

            return await _context.Users
                .AsNoTracking() 
                .Where(c => topCustomerIds.Contains(c.Id))
                .ToListAsync();
        }
        public async Task<User> GetUserWithOrders(int id)
        {
            return await _context.Users.Include(n => n.Orders).FirstOrDefaultAsync(n=>n.Id==id);
        }
        public async Task<bool> IsRoleExit(int Id)
        {
            return await _context.Roles.AnyAsync(n => n.Id == Id);
        }

        public async Task<User> GetUserByEmail(string Email)
        {
          return await _context.Users.FirstOrDefaultAsync(N => N.Email == Email.Trim());
           
        }
        public Task<bool> IsUserNameExist(string UserName)
        {
            return _context.Users.AnyAsync(N => N.UserName == UserName.Trim());
        }
    }
}
