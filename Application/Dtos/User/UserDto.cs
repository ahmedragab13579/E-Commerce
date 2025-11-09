using Application.Dtos.Carts;
using Application.Dtos.Order;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.User
{
    public class UserDto
    {
        public int Id { get; private set; }

        [Required, MaxLength(100)]
        public string FirstName { get; private set; }
        [Required, MaxLength(100)]
        public string LastName { get; private set; }

        [Required, MaxLength(100)]
        public string UserName { get; private set; }
        [Required, MaxLength(200)]
        [EmailAddress]
        public string Email { get; private set; }
        [MaxLength(15)]
        [RegularExpression(@"^(\+?\d{1,3})?\d{10,15}$", ErrorMessage = "Invalid phone number format.")]
        public string? Phone { get; private set; }
        public string PasswordHash { get; private set; }

        public int RoleId { get; private set; }
        public Role Role { get; private set; }

        public bool IsBlocked { get; private set; }
        public bool IsDeleted { get; private set; }
        public string? IsDeletedReason { get; private set; }
        public string? BlockedReason { get; private set; }
        
        public ICollection<OrderDto> Orders { get; private set; } = new HashSet<OrderDto>();
        public Cart Cart { get; private set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

    }
}