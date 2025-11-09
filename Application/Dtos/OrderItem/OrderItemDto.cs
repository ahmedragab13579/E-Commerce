using Application.Dtos.Order;   
using Application.Dtos.Product; 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.OrderItem
{
    public class OrderItemDto
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

       
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        public ProductDto Product { get; set; } 

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 10000, ErrorMessage = "Quantity must be at least 1.")] 
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit price is required.")]
        [Range(0.01, 1000000.00, ErrorMessage = "Unit price must be greater than zero.")] 
        public decimal UnitPrice { get; set; }
    }
}