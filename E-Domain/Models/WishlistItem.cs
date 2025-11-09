using System;
using System.ComponentModel.DataAnnotations; 

namespace E_Domain.Models
{
    public class WishlistItem
    {
        public int Id { get; private set; }

        public int WishlistId { get; private set; }
        public Wishlist Wishlist { get; private set; } 

        public int ProductId { get; private set; }
        public Product Product { get; private set; } 
        private WishlistItem() { }

        public WishlistItem(int wishlistId, Product product)
        {
            if (wishlistId <= 0)
                throw new ArgumentException("WishlistId must be a positive integer.", nameof(wishlistId));
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            if (product.IsDeleted == true) 
                throw new InvalidOperationException("Cannot add a deleted product to the wishlist.");


            WishlistId = wishlistId;
            ProductId = product.Id;
            Product = product;
        }
    }
}