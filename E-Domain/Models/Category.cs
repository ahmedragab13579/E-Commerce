using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Domain.Models
{
    public class Category
    {
        public int Id { get;private set; }

        [Required, MaxLength(30)]
        public string Name { get; private set; }
        [Required, MaxLength(500)]

        public string Description { get; private set; }

        public bool? IsDeleted { get; private set; } = false;
        public ICollection<Product> Products { get; private set; } = new HashSet<Product>();

        private Category()
        {
            
        }
        public Category(string Name,string Description)
        {
            if(string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentException("Category name cannot be null or empty.", nameof(Name));
            }
            if(Name.Length > 30)
            {
                throw new ArgumentException("Category name cannot exceed 100 characters.", nameof(Name));
            }
            if(Description != null && Description.Length > 500)
            {
                throw new ArgumentException("Category description cannot exceed 500 characters.", nameof(Description));
            }
            if(string.IsNullOrWhiteSpace(Description))
            {
                throw new ArgumentException("Category description cannot be null or empty.", nameof(Description));
            }
            this.Name = Name;
            this.Description = Description;
        }

        public void DeleteCategory()
        {
            if (this.IsDeleted == true)
            {
                throw new InvalidOperationException("Category is already deleted.");
            }
            this.IsDeleted = true;
        }

        public void UpdateCategory(string name,string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be null or empty.", nameof(name));
            }
            if (name.Length > 30)
            {
                throw new ArgumentException("Category name cannot exceed 100 characters.", nameof(name));
            }
            if (description != null && description.Length > 500)
            {
                throw new ArgumentException("Category description cannot exceed 500 characters.", nameof(description));
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Category description cannot be null or empty.", nameof(description));
            }
            this.Name = name;
            this.Description = description;
        }
    }
}
