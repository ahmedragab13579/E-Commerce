using System;
using System.Collections.Generic;
using System.Linq; 

namespace E_Domain.Models
{
    public class Wishlist
    {
        public int Id { get; private set; } 
        public int UserId { get; private set; }
        public User User { get; private set; } 

        public ICollection<WishlistItem> WishlistItems { get; private set; } = new HashSet<WishlistItem>();

        private Wishlist() { }

        public Wishlist(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            UserId = userId;
        }

        public void AddItem(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            if (WishlistItems.Any(item => item.ProductId == product.Id))
            {
                throw new InvalidOperationException($"Product '{product.Name}' is already in the wishlist.");
            }
            if(product.IsDeleted.Value)
            {
                throw new InvalidOperationException($"Cannot add deleted product '{product.Name}' to the wishlist.");
            }

            var newItem = new WishlistItem(this.Id, product);
            WishlistItems.Add(newItem);
        }

        public void ClearWishlist()
        {
            WishlistItems.Clear();
        }
     
        
        public bool IsProductInIt(int productId)
        {
            if (!WishlistItems.Any(item => item.ProductId == productId))
            {
                return false;
            }
            return true;
        }



        public void RemoveItem(int productId)
        {
            var itemToRemove = WishlistItems.FirstOrDefault(item => item.ProductId == productId);

            if (itemToRemove != null)
            {
                WishlistItems.Remove(itemToRemove);
            }
             else
            {
                throw new InvalidOperationException($"Product with ID {productId} not found in the wishlist.");
            }
        }
    }
}