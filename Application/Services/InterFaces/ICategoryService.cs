using Application.Dtos.Category;
using Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces
{
    public interface ICategoryService
    {
        Task <Result<int>> Add(AddCategoryDto entity);
        Task<Result<bool>> Update(int id,UpdateCategoryDto category);


        Task<Result<bool>> Delete(int id);
        Task<Result<CategoryDto>> GetById(int id);
        Task<Result<List<CategoryDto>>> GetAll();

    }
}
