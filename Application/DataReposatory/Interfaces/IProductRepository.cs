
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces
{
    public interface IProductRepository: IGenericRepository<Product>
    {

        void UpdateRange(IList<Product> entitis); Task<bool> IsProductHasAnyOrders(int productid);
        Task<IList<Product>> GetProductsByIdsAsync(List<int> Ids);
        Task<IEnumerable<Product>> GetAllAsyncWithFilter(string Name);
        Task<List<Product>> GetBestSellingProductsAsync(int count = 10);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);


    


    }
}
