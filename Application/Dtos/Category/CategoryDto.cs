using Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")] 
        public string Description { get; set; }

        public ICollection<ProductDto> Products { get; set; } = new HashSet<ProductDto>();
    }
}
