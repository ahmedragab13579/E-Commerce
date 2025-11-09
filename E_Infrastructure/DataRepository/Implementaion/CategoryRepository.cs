using Application.DataReposatory.Interfaces;
using E_Domain.Models;
using E_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DbSet<Category> _categories;
        public CategoryRepository(E_ApplicationDbContext _context):base(_context)
        {
            _categories=this._context.Categories;
            
        }

        public async Task<bool> IsCategoryExist(string name)
        {
            return await _categories.AnyAsync(c => c.Name == name);
        }

    
        public async Task<bool> HasProducts(int categoryId)
        {
            return await _context.Products.AnyAsync(p=>p.CategoryId==categoryId);

        }
    }
}
