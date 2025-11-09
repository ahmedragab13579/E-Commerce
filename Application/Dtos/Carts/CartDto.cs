using Application.Dtos.CartItem;
using Application.Dtos.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Carts
{
    public class CartDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }
        public UserDto User { get; set; }

        public DateTime LastPersisted { get; set; } = DateTime.UtcNow; 

        public ICollection<CartItemDto> CartItems { get; set; } = new HashSet<CartItemDto>();
    }
}
