using Application.DataReposatory.Interfaces;
using E_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly E_ApplicationDbContext _context;

        public GenericRepository(E_ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task  Delete(int Id)
        {
          await _context.Users
                    .Where(u => u.Id == Id)
                    .ExecuteUpdateAsync(updates => updates.SetProperty(u => u.IsDeleted, true));

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
