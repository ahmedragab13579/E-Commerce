using Application.Dtos.Category;
using Application.Services.InterFaces;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreApi.MainControllers.AdminControllers.Category
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Fetches all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        /// <response code="200">Returns the list of categories.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAll();
            return ActionResultStatus.MapResult(categories);
        }

        /// <summary>
        /// Fetches a specific category by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>The category details if found.</returns>
        /// <response code="200">Returns the requested category.</response>
        /// <response code="404">If the category with the specified ID is not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            return ActionResultStatus.MapResult(category);
        }

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="addCategoryDto">The category data to add.</param>
        /// <returns>The newly created category.</returns>
        /// <response code="201">Returns the newly created category.</response>
        /// <response code="400">If the category data is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDto addCategoryDto)
        {
            var addCategory = await _categoryService.Add(addCategoryDto);
            return ActionResultStatus.MapResult(addCategory);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="updateCategoryDto">The updated category data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the category was updated successfully.</response>
        /// <response code="404">If the category to update is not found.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            var updateCategory = await _categoryService.Update(id, updateCategoryDto);
            return ActionResultStatus.MapResult(updateCategory);
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the category was deleted successfully.</response>
        /// <response code="404">If the category to delete is not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleteCategory = await _categoryService.Delete(id);
            return ActionResultStatus.MapResult(deleteCategory);
        }
    }
}