using Application.Dtos.User;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Wishlist
{
    public class WishListDto
    {
        public int Id { get;  set; }
        [Required]
        public int UserId { get;  set; }
        public UserDto User { get;  set; }

        public ICollection<WishlistItem> WishlistItems { get;  set; } = new HashSet<WishlistItem>();

    }
}
