using Application.DataReposatory.Interfaces;
using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.Category;
using Application.Results;
using Application.Services.InterFaces;
using Application.Services.InterFaces.Mapping;
using E_Domain.Models;
using E_Infrastructure.Data;

namespace E_Infrastructure.Services.Implementaions
{
    public class CategoryService : ICategoryService
    {
        private readonly IMappingServices _mappingServices;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService( IMappingServices _mappingServices,IUnitOfWork unitOfWork)
        {
            this._mappingServices = _mappingServices;
            _unitOfWork = unitOfWork;


        }


        public async Task<Result<int>> Add(AddCategoryDto entity)
        {
            if (await _unitOfWork.Category.IsCategoryExist(entity.Name))
            {
                return Result<int>.Fail("DUPLICATE_NAME", "A category with this name already exists.");
            }
            try
            {
                var NewCategory = new Category(entity.Name, entity.Description);
                await _unitOfWork.Category.AddAsync(NewCategory);
                var Success = await _unitOfWork.CompleteTask();
                if (Success != 0)
                {
                    return Result<int>.Ok(Success, "Category Added Successfully");
                }
                return Result<int>.Fail("NOT_ADD", "Category Not Add ");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("EXCEPTION", $"An error occurred: {ex.Message}");


            }
        }

    
        public async Task<Result<bool>> Update(int id,UpdateCategoryDto categoryDto)
        {
            var existingCategory = await _unitOfWork.Category.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return Result<bool>.Fail("NOT_FOUND", "Category Not Found");
            }
            existingCategory.UpdateCategory(categoryDto.Name, categoryDto.Description);
            _unitOfWork.Category.Update(existingCategory);
            var isUpdated = await _unitOfWork.CompleteTask() > 0;
            if (isUpdated)
            {
                return Result<bool>.Ok(true, "Updated Successfully");
            }

                return Result<bool>.Fail("NOT_FOUND", "Category Not Found");


        }



        public async Task<Result<bool>> Delete(int id)
        {
            var existingCategory = await _unitOfWork.Category.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return Result<bool>.Fail("NOT_FOUND", "Category Not Found");
            }
            try
            {
                if( await _unitOfWork.Category.HasProducts(id))
                {
                    return Result<bool>.Fail("CATEGORY_HAS_PRODUCTS", "Cannot delete category with associated products.");
                }
                existingCategory.DeleteCategory();
                _unitOfWork.Category.Update(existingCategory);
                var isdeleted = await _unitOfWork.CompleteTask() > 0;
                if (isdeleted)
                {
                    return Result<bool>.Ok(true, "Deleted Successfully");
                }
                return Result<bool>.Fail("SAVE_FAILED", "Category Not Saved");

            }
            catch (InvalidOperationException ex)
            {
                return Result<bool>.Fail("INVALID_OPERATION", ex.Message);
            }
            catch (Exception ex) 
            {
                return Result<bool>.Fail("SERVER_ERROR", ex.Message);
            }

        }



        public async Task<Result<List<CategoryDto>>> GetAll()
        {
            var categories = await _unitOfWork.Category.GetAllAsync();
            var categoryDtos = _mappingServices.MapList<Category, CategoryDto>(categories);
            return Result<List<CategoryDto>>.Ok(categoryDtos.ToList(), "Categories Retrieved Successfully");
        }




        public async Task<Result<CategoryDto>> GetById(int id)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(id);
            if (category == null)
            {
                return Result<CategoryDto>.Fail("NOT_FOUND", "Category Not Found");
            }
            var categoryDto = _mappingServices.Map<Category, CategoryDto>(category);
            return Result<CategoryDto>.Ok(categoryDto, "Category Retrieved Successfully");
        }
    }
}
