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
    public class RoleRepository : GenericRepository<E_Domain.Models.Role>, IRoleRepository
    {
        private readonly DbSet<E_Domain.Models.Role> _dbSet;
        public RoleRepository(E_ApplicationDbContext _context):base(_context)
        {
            _dbSet = _context.Set<E_Domain.Models.Role>();

        }
        public async Task<E_Domain.Models.Role> GetByNameAsync(string Name)
        {
            return await _dbSet.FirstOrDefaultAsync(n => n.Name == Name);
        }

        public async Task<bool> IsRoleExist(string Name)
        {
            return await _dbSet.AnyAsync(n => n.Name == Name);
        }
    }
}
