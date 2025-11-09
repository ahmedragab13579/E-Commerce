using Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Wishlist
{
    public class WishListItemDto
    {
        public int Id { get; private set; }
        [Required]
        public int WishlistId { get; private set; }
        public WishListDto Wishlist { get; private set; }
        [Required]
        public int ProductId { get; private set; }
        public ProductDto Product { get; private set; }
    }
}
