using E_Domain.Enums; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Domain.Models
{
    public class Payment
    {
        public int Id { get; private set; }
        public int OrderId { get; private set; }
        public Order Order { get; private set; }

        [Required]
        public string PaymentMethod { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 100000.00, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; private set; }

        public DateTime PaymentDate { get; private set; }

        private Payment()
        {
        }

        public Payment(Order order, string paymentMethod)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                throw new ArgumentException("Payment method cannot be null or empty.", nameof(paymentMethod));
            }

            if (order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Cannot create a payment for an order that is not in 'Pending' status.");
            }

            if (order.TotalAmount <= 0)
            {
                throw new InvalidOperationException("Order total amount must be greater than zero to process payment.");
            }

            OrderId = order.Id;
            Order = order;
            PaymentMethod = paymentMethod;
            PaymentDate = DateTime.UtcNow;

            Amount = order.TotalAmount;

            order.UpdateStatus(OrderStatus.Paid);
        }
    }
}