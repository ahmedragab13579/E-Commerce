using Application.DataReposatory.Interfaces;
using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.Product;
using Application.Results;
using Application.Services.InterFaces;
using Application.Services.InterFaces.Mapping;
using E_Domain.Models;


namespace E_Infrastructure.Services.Implementaions
{
    public class ProductService : IProductService
    {
        private readonly IMappingServices _mappingServices;
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork, IMappingServices _mappingServices)
        {
            this._mappingServices = _mappingServices;
            _unitOfWork = unitOfWork;

        }
        public async Task<Result<int>> Add(AddProductDto entity)
        {
            try
            {
                var product = new Product(entity.Name, entity.Description, entity.Price, entity.Stock, entity.CategoryId, entity.ImagePath);
                await _unitOfWork.Products.AddAsync(product);
                var id = await _unitOfWork.CompleteTask();
                if (id > 0)
                {
                    return Result<int>.Ok(product.Id, "Product Added Successfully");
                }
                return Result<int>.Fail("NOT_ADD", "Product Not Add ");
            }
            catch(ArgumentOutOfRangeException ex)
            {
                return Result<int>.Fail("EXCEPTION", $"An error occurred: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return Result<int>.Fail("EXCEPTION", $"An error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("EXCEPTION", $"An unexpected error occurred: {ex.Message}");
            }
        }
   
        
        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                if (await _unitOfWork.Products.IsProductHasAnyOrders(id))
                {
                    return Result<bool>.Fail("HAS_DATA", "Product was attach with orders data");

                }
                Product Product = await _unitOfWork.Products.GetByIdAsync(id);
                if (Product == null)
                    return Result<bool>.Fail("NOT_FOUND", "Product Not Found");
                Product.MarkAsDeleted();
                _unitOfWork.Products.Update(Product);
                var success = await _unitOfWork.CompleteTask() > 0;
                if (success)
                    return Result<bool>.Ok(true, "Product Deleted Successfully");
                return Result<bool>.Fail("SAVE_FAILED", "Product Not Saved");
            }
            catch(Exception ex) 
            {
                return Result<bool>.Fail("EXEPTION", ex.Message);

            }
           

        }


        public async Task<Result<IEnumerable<ProductDto>>> GetAll()
        {
            var Products = await _unitOfWork.Products.GetAllAsync();
            if (Products!=null)
            {
                return Result<IEnumerable<ProductDto>>.Ok(_mappingServices.MapList<Product, ProductDto>(Products), "Return All Products");
            }
            return Result<IEnumerable<ProductDto>>.Fail("NO_ITEMS", "There Is No Product");
        }
        public async Task<Result<IEnumerable<ProductDto>>> GetBestSellingProductsAsync()
        {
            var Products = await _unitOfWork.Products.GetBestSellingProductsAsync();
            if (Products!=null)
            {
                return Result<IEnumerable<ProductDto>>.Ok(_mappingServices.MapList<Product, ProductDto>(Products), "Return All Products");
            }
            return Result<IEnumerable<ProductDto>>.Fail("NO_ITEMS", "There Is No Product");
        }
        public async Task<Result<IEnumerable<ProductDto>>> GetAllWithFilter(string Name)
        {
            IEnumerable<Product> Products = await _unitOfWork.Products.GetAllAsyncWithFilter(Name);
            if (Products!=null)
            {
                return Result<IEnumerable<ProductDto>>.Ok(_mappingServices.MapList<Product,ProductDto>(Products), "Return All Filterd Products");
            }
            return Result<IEnumerable<ProductDto>>.Fail("NO_ITEMS", "There Is No Product");
        }

        public async Task<Result<ProductDto>> GetById(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product!=null)
            {
                return Result<ProductDto>.Ok(_mappingServices.Map<Product, ProductDto>(product), "Product Was Found");
            }
            return Result<ProductDto>.Fail("NOT_FOUND", "Product Not Found");
        }

        public async Task<Result<bool>> Update(int id,UpdateProductDto entity)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(id);
                if (product != null)
                {
                    product.UpdateProductInfromation(entity.Name, entity.Description, entity.Stock, entity.Price);
                    _unitOfWork.Products.Update(product);
                    var success = await _unitOfWork.CompleteTask() > 0;
                    return Result<bool>.Ok(success, "Updated Successfully");
                }
                return Result<bool>.Fail("NOT_FOUND", "Product Not Found");
            }
            catch(InvalidOperationException ex)
            {
                return Result<bool>.Fail("EXEPTION", ex.Message);

            }
            catch(Exception ex)
            {
                return Result<bool>.Fail("EXEPTION", ex.Message);
            }

        }

        public async Task<Result<IEnumerable<ProductDto>>> GetProductsByCategoryId(int categoryId)
        {
            var categoryProducts = await _unitOfWork.Products.GetProductsByCategoryIdAsync(categoryId);
            if (categoryProducts!=null)
            {
                return Result<IEnumerable<ProductDto>>.Ok(_mappingServices.MapList<Product, ProductDto>(categoryProducts), "Category Was Exist");
            }
            return Result<IEnumerable<ProductDto>>.Fail("NOT_FOUND", "Category Not Found Or Wrong ID");
        }

    }
}

