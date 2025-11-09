using Application.Dtos.OrderItem;
using Application.Dtos.Payment;
using Application.Dtos.User;
using E_Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Order
{
    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "Order status is required.")]
        [MaxLength(50)]
        public string NewStatus { get; set; }
        [Required(ErrorMessage = "Order status is required.")]
        [MaxLength(50)]
        public string OldStatus { get; set; }

    }
}
