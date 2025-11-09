using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Domain.Models
{
    public class Cart
    {
        public int Id { get;private set; }

        public int UserId { get; private set; }
        public User User { get; private set; }

        public ICollection<CartItem> CartItems { get; private set; } = new HashSet<CartItem>();

        private Cart()
        {
        }
        public Cart(int userId)
        {
            if(userId<=0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "UserId must be greater than zero.");
            }
            UserId = userId;
        }
        public void SyncItems(ICollection<CartItem> sessionItems)
        {
            CartItems.Clear();
            foreach (var item in sessionItems)
            {
                CartItems.Add(item);
            }
        }
    
        public bool AddItem(Product product, int quantity)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
            }
            var existingItem = CartItems.FirstOrDefault(ci => ci.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            }
            else
            {
                var newItem = new CartItem(this.Id, product, quantity);
                CartItems.Add(newItem);
            }
            return true;
        }

        public void UpdateItemQuantity(int productId, int newQuantity)
        {
            var item = CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (item == null)
            {
                throw new InvalidOperationException("Item not found in cart.");
            }
            item.UpdateQuantity(newQuantity);
        }


        public void RemoveItem(int productId)
        {
            var item = CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (item != null)
            {
                CartItems.Remove(item);
            }

        }
    }
}
