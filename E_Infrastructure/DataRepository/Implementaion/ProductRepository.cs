using Application.DataReposatory.Interfaces;
using E_Domain.Models;
using E_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DbSet<Product> _products;
        public ProductRepository(E_ApplicationDbContext _context):base(_context)
        {
            _products = this._context.Products;
            
        }
     

   
        public async Task<bool> IsProductHasAnyOrders(int productid)
        {
            return await _context.OrderItems.AnyAsync(x=>x.ProductId==productid);

        }

        public async Task<IEnumerable<Product>> GetAllAsyncWithFilter(string Name)
        {
            var searchTerm = Name.ToLower();

            return await _products.AsNoTracking()
                .Where(n => n.Name.ToLower().Contains(searchTerm)) 
                .Where(n => n.IsDeleted == false)
                .ToListAsync();
        }

        public async Task <IList<Product>> GetProductsByIdsAsync(List<int> Ids)
        {
            return await _products.AsNoTracking().Where(n => Ids.Contains(n.Id)).Where(n => n.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _products.AsNoTracking().Where(n => n.CategoryId == categoryId).Where(n=>n.IsDeleted==false).ToListAsync();
        }

        public void UpdateRange(IList<Product> entities)
        {
          
           _products.UpdateRange(entities);
        }

        public async Task<List<Product>> GetBestSellingProductsAsync(int count = 100)
        {
            
            var topProductIds = await _context.OrderItems
                .GroupBy(oi => oi.ProductId) 
                .Select(g => new {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(oi => oi.Quantity) 
                })
                .OrderByDescending(x => x.TotalQuantity) 
                .Take(count) 
                .Select(x => x.ProductId) 
                .ToListAsync();

            return await _context.Products
                .AsNoTracking()
                .Where(p => topProductIds.Contains(p.Id))
                .ToListAsync();
        }
    }
}


