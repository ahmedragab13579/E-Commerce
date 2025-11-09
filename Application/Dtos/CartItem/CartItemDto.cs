using Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.CartItem
{
    public class CartItemDto
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10.")] 
        public int Quantity { get; set; }
    }
}
