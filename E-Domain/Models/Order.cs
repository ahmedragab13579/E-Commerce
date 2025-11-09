using E_Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace E_Domain.Models
{
    public class Order
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public DateTime OrderDate { get; private set; }
        [Required]
        public OrderStatus Status { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 100000.00, ErrorMessage = "Amount must be greater than zero.")]
        public decimal TotalAmount { get; private set; }

        public ICollection<OrderItem> OrderItems { get; private set; } = new HashSet<OrderItem>();
        public Payment Payment { get; private set; }

        private Order()
        {
        }

        public Order(int userId, ICollection<CartItem> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
            {
                throw new ArgumentException("Order must contain at least one item.", nameof(cartItems));
            }

            UserId = userId;
            Status = OrderStatus.Pending; 
            OrderDate = DateTime.UtcNow;

            decimal calculatedTotal = 0;
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem(item.Product, item.Quantity);
                OrderItems.Add(orderItem);

                calculatedTotal += orderItem.UnitPrice * orderItem.Quantity;
            }

            if (calculatedTotal <= 0)
            {
                throw new InvalidOperationException("Calculated total amount must be greater than zero.");
            }
            TotalAmount = calculatedTotal; 
        }


        public void UpdateStatus(OrderStatus newStatus)
        {
            var currentStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), this.Status.ToString());

           
            if (currentStatus == OrderStatus.Shipped && newStatus == OrderStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot cancel an order that has already been shipped.");
            }

            if (currentStatus == OrderStatus.Pending && newStatus == OrderStatus.Shipped)
            {
                throw new InvalidOperationException("Cannot ship an order that has not been paid for.");
            }

            if (currentStatus == OrderStatus.Cancelled)
            {
                throw new InvalidOperationException("This order has been cancelled.");
            }


            this.Status = newStatus;
        }

  


    }
}