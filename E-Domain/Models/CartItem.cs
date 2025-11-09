using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Domain.Models
{
    public class CartItem
    {
        public int Id { get;private set; }

        public int CartId { get; private set; }
        public Cart Cart { get; private set; }

        public int ProductId { get; private set; }
        public Product Product { get; private set; }

        [Range(1, 100000.00, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; private set; }

        private CartItem()
        {
            
        }

    

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newQuantity), "Quantity must be greater than zero.");
            }
            if (Product.Stock < newQuantity)
            {
                throw new InvalidOperationException("Not enough stock available for this product.");
            }
            Quantity = newQuantity;
        }


        public CartItem(int cartid, Product product,int Quantity)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }
            if (Quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Quantity), "Quantity must be greater than zero.");
            }
            if (product.Stock < Quantity)
            {
                throw new InvalidOperationException("Not enough stock available for this product.");
            }
            CartId = cartid;
            this.Product = product;
            this.Quantity = Quantity;
            this.ProductId = product.Id;
        }
    }
}
