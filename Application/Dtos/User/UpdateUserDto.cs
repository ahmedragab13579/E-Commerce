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
    public class UpdateUserDto
    {


        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }

     

        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [RegularExpression(@"^(\+2)?01[0125][0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number format.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? Phone { get; set; }


    }
}
