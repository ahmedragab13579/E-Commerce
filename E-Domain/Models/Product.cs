using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Domain.Models
{
    public class Product
    {
        public int Id { get; private set; } 

        [Required, MaxLength(200)]
        public string Name { get; private set; } 

        [MaxLength(500)]
        public string Description { get; private set; } 

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 100000.00, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; private set; } 

        [Range(0, 100000, ErrorMessage = "Stock must be zero or greater.")] 
        public int Stock { get; private set; } 

        public int CategoryId { get; private set; } 
        public Category Category { get; private set; }

        public string? ImagePath { get; private set; } 
        public bool? IsDeleted { get; private set; } = false;

        public ICollection<ProductRecommendation> PrimaryRecommendations { get; set; } = new HashSet<ProductRecommendation>();
        public ICollection<ProductRecommendation> RecommendedAs { get; set; } = new HashSet<ProductRecommendation>();


        public ICollection<CartItem> CartItems { get; private set; } = new HashSet<CartItem>();
        public ICollection<OrderItem> OrderItems { get; private set; } = new HashSet<OrderItem>();

        [Timestamp]
        public byte[] RowVersion { get; private set; }

        private Product()
        {
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0) 
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity to decrease must be positive.");
            }
            if (this.Stock < quantity) 
            {
                throw new InvalidOperationException($"Insufficient stock for product {this.Name}. Only {this.Stock} available.");
            }
            this.Stock -= quantity;
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0) 
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity to increase must be positive.");
            }
            this.Stock += quantity;
        }

        public void UpdateProductInfromation(string Name,string Description, int stock,decimal Price)
        {
            if (stock <= 0)
                throw new InvalidOperationException("Stock Must Be Greater Than Zero");
            if (string.IsNullOrWhiteSpace(Name)) 
                throw new ArgumentException("Product name cannot be empty.", nameof(Name));
            if (Price <= 0)
                throw new ArgumentOutOfRangeException(nameof(Price), "Price must be greater than zero.");
            if (string.IsNullOrEmpty(Description)) 
                throw new ArgumentOutOfRangeException(nameof(Description), "Product Description cannot be empty.");

            this.Name = Name;
            this.Description = Description;
            this.Stock = stock;
            this.Price = Price;
        }
        public bool MarkAsDeleted()
        {
            if (IsDeleted == true)
                return false;
            IsDeleted = true;
            return true;
        }

        public Product(string name, string description, decimal price, int stock, int categoryId, string? imagePath = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than zero.");
            if (stock < 0)
                throw new ArgumentOutOfRangeException(nameof(stock), "Stock cannot be negative.");
            if (categoryId <= 0)
                throw new ArgumentOutOfRangeException(nameof(categoryId), "Invalid Category ID.");

            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
            ImagePath = imagePath;
            IsDeleted = false; 
        }

       
  
    }
}