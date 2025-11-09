using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace E_Domain.Models
{
    public class User
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

        public ICollection<Order> Orders { get; private set; } = new HashSet<Order>();
        public Cart Cart { get; private set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        private User()
        {
        }

        public User(string firstName, string lastName, string userName, string email, string? phone, string passwordHash) 
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username is required.", nameof(userName));
            if (string.IsNullOrWhiteSpace(email)) 
                throw new ArgumentException("Email is required.", nameof(email));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash is required.", nameof(passwordHash));

            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            Phone = phone; 
            PasswordHash = passwordHash;
            RoleId = 2; 
            IsBlocked = false;
            IsDeleted = false;
        }


        public void UpdateProfile(string firstName, string lastName, string userName, string email, string? phone)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username is required.", nameof(userName));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            FirstName = firstName;
            LastName = lastName;
            UserName = userName; 
            Email = email;       
            Phone = phone;
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash is required.", nameof(newPasswordHash));
            PasswordHash = newPasswordHash;
        }

        public void Block(string reason = "No reason provided.")
        {
            if (!IsBlocked)
            {
                IsBlocked = true;
                BlockedReason = reason;
            }
        }

        public void Unblock()
        {
            if (IsBlocked)
            {
                IsBlocked = false;
                BlockedReason = null;
            }
        }

        public void SoftDelete(string reason = "No reason provided.")
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                IsDeletedReason = reason;
               
            }
        }

        public void Restore()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
                IsDeletedReason = null;
                
            }
        }

        public void ChangeRole(int newRoleId)
        {
            if (IsDeleted)
            {
                throw new InvalidOperationException("Cannot change role of a deleted user.");
            }
            RoleId = newRoleId;
        }
    }
}