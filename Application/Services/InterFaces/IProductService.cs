using Application.Dtos.Product;
using Application.Results;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces
{
    public interface IProductService
    {
        Task<Result<int>> Add(AddProductDto entity);

        Task<Result<bool>> DeleteAsync(int id);

        Task<Result<IEnumerable<ProductDto>>> GetAll();

        Task<Result<IEnumerable<ProductDto>>> GetAllWithFilter(string Name);

        Task<Result<ProductDto>> GetById(int id);
        Task<Result<IEnumerable<ProductDto>>> GetBestSellingProductsAsync();


        Task<Result<bool>> Update(int id,UpdateProductDto entity);

        Task<Result<IEnumerable<ProductDto>>> GetProductsByCategoryId(int categoryId);
     

    }
}
