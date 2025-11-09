using Application.Dtos.OrderItem; 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Order
{
    public class AddOrderDto
    {
        
        public int UserId { get; set; }
        [Required(ErrorMessage = "Order items are required.")]
        [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
        public ICollection<AddOrderItemDto> OrderItems { get; set; } = new HashSet<AddOrderItemDto>();

     
    }
}