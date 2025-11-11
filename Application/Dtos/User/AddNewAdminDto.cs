using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User
{
    public class AddNewAdminDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; }
        [Required, MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(15)]
        [RegularExpression(@"^(\+?\d{1,3})?\d{10,15}$", ErrorMessage = "Invalid phone number format.")]
        public string? Phone { get; set; }
        [Required, MinLength(8),MaxLength(15)]
        public string Password { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        public int RoleId { get; set; }

    }
}
