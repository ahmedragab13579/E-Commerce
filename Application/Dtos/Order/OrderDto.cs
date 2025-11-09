using Application.Dtos.OrderItem; 
using Application.Dtos.Payment;   
using Application.Dtos.User;      
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Order
{
    public class OrderDto
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        public UserDto User { get; set; } 

        public DateTime OrderDate { get; set; } 

        [Required(ErrorMessage = "Order status is required.")]
        [MaxLength(50)] 
        public string Status { get; set; }

        [Range(0.01, 1000000.00, ErrorMessage = "Total amount must be greater than zero.")] 
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItemDto> OrderItems { get; set; } = new HashSet<OrderItemDto>();

        public PaymentDto? Payment { get; set; } 
    }
}