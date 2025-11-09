using Application.Dtos.Order;   
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Payment
{
    public class PaymentDto
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

      

        [Required(ErrorMessage = "Payment method is required.")]
        [MaxLength(50, ErrorMessage = "Payment method cannot exceed 50 characters.")] 
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, 1000000.00, ErrorMessage = "Amount must be greater than zero.")] 
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}