# E-Commerce Project in ASP.NET

A comprehensive e-commerce application built with ASP.NET, designed to provide a complete online shopping experience with product management, user authentication, shopping cart functionality, and order processing.

## 📋 Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Technology Stack](#technology-stack)
- [Database Architecture](#database-architecture)
- [Installation & Setup](#installation--setup)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Screenshots](#screenshots)
- [Contributing](#contributing)
- [License](#license)

---

## ✨ Features

- 🛍️ **Product Management** - Browse, search, and filter products
- 👤 **User Authentication** - Secure login and registration system
- 🛒 **Shopping Cart** - Add/remove items and manage cart
- 💳 **Order Processing** - Complete checkout and order management
- 📊 **Admin Dashboard** - Manage products, users, and orders
- ⭐ **Reviews & Ratings** - Customer feedback system
- 🔐 **Security** - JWT authentication and encrypted passwords
- 📱 **Responsive Design** - Mobile-friendly interface

---

## 📁 Project Structure

```
E-Commerce/
├── Models/              # Data models
├── Controllers/         # API controllers
├── Services/           # Business logic
├── Data/              # Database context
├── Views/             # UI views
├── wwwroot/           # Static files
├── appsettings.json   # Configuration
└── Program.cs         # Application entry point
```

---

## 🛠️ Technology Stack

- **Backend**: ASP.NET Core (.NET 6+)
- **Language**: C# (96.8%), Python (3.2%)
- **Database**: SQL Server / Entity Framework Core
- **Frontend**: HTML5, CSS3, JavaScript
- **Authentication**: JWT Tokens
- **Package Manager**: NuGet

---

## 🗄️ Database Architecture

### Database Diagram

![Database Diagram](https://i.suar.me/Mpp13/l)

The database includes the following main entities:

- **Users** - Customer and admin user information
- **Products** - Product catalog and inventory
- **Categories** - Product categorization
- **Orders** - Customer orders and transactions
- **OrderItems** - Individual items in orders
- **Reviews** - Customer product reviews
- **ShoppingCart** - User shopping cart items

---

## 💻 Installation & Setup

### Prerequisites

- .NET 6 or higher
- SQL Server (local or remote)
- Visual Studio 2022 or VS Code
- Git

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/ahmedragab13579/E-Commerce.git
   cd E-Commerce
   ```

2. **Update Database Connection**
   - Open `appsettings.json`
   - Update the connection string with your SQL Server details

3. **Install Dependencies**
   ```bash
   dotnet restore
   ```

4. **Apply Database Migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the Application**
   ```bash
   dotnet run
   ```

6. **Access the Application**
   - Open browser and navigate to `https://localhost:5001`

---

## 🚀 Usage

### 1. Home Page

![Home Page](https://i.suar.me/rgOO1/l)

Welcome screen with featured products and navigation.

### 2. Product Listing

![Product Listing](https://i.suar.me/7nPP0/l)

Browse all products with pagination and filtering options.

### 3. Product Details

![Product Details](https://i.suar.me/ogqql/l)

Detailed product information including description, price, and reviews.

### 4. Shopping Cart

![Shopping Cart](https://i.suar.me/xzool/l)

Review items in cart before checkout with quantity adjustment.

### 5. Checkout Process

![Checkout](https://i.suar.me/8zVeZ/l)

Complete multi-step checkout with shipping and payment information.

### 6. Order Confirmation

![Order Confirmation](https://i.suar.me/8zVeZ/l)

Order success page with confirmation details and tracking info.

### 7. Admin Dashboard

![Admin Dashboard](https://i.suar.me/3zVVr/l)

Admin control panel for managing store operations.

### 8. User Profile

![User Profile](https://i.suar.me/YQrrZ/l)

User account settings and order history view.

### 9. Product Management

![Product Management](https://i.suar.me/zXKKW/l)
![Product Management](https://i.suar.me/Jp774/l)
![Product Management](https://i.suar.me/4z44Q/l)
Admin section for adding, editing, and deleting products.

### 10. Order Management

![Order Management](https://i.suar.me/9zVVX/l)

Admin section for tracking and managing customer orders.

### 11. Wish List

![Search Feature](https://i.suar.me/rgOO1/l)

Search products by name, category, or keywords.

### 14. User Authentication

![User Authentication](https://i.suar.me/wzrr8/l)
![User Authentication](https://i.suar.me/V9jjp/l)

Secure login and registration interface with validation.

### 16. Cagegory Management

![Inventory Management](https://i.suar.me/g433X/l)
![Inventory Management](https://i.suar.me/Mp22P/l)

Stock tracking and inventory level management for products.

---

## 📡 API Endpoints

### Authentication
```
POST   /api/auth/register        - Register new user
POST   /api/auth/login           - Login user
POST   /api/auth/logout          - Logout user
POST   /api/auth/refresh-token   - Refresh JWT token
```

### Products
```
GET    /api/products             - Get all products
GET    /api/products/{id}        - Get product by ID
POST   /api/products             - Create product (Admin)
PUT    /api/products/{id}        - Update product (Admin)
DELETE /api/products/{id}        - Delete product (Admin)
GET    /api/categories           - Get all categories
GET    /api/products/search      - Search products
```

### Shopping Cart
```
GET    /api/cart                 - Get user's cart
POST   /api/cart/add             - Add item to cart
PUT    /api/cart/{itemId}        - Update cart item quantity
DELETE /api/cart/{itemId}        - Remove item from cart
DELETE /api/cart                 - Clear entire cart
```

### Orders
```
GET    /api/orders               - Get user's orders
GET    /api/orders/{id}          - Get order details
POST   /api/orders               - Create order (Checkout)
PUT    /api/orders/{id}          - Update order status (Admin)
DELETE /api/orders/{id}          - Cancel order
GET    /api/orders/status/{id}   - Get order status
```

### Reviews
```
GET    /api/reviews/{productId}  - Get product reviews
POST   /api/reviews              - Create review
PUT    /api/reviews/{id}         - Update review
DELETE /api/reviews/{id}         - Delete review
GET    /api/reviews/rating/{productId} - Get product rating
```

### Users
```
GET    /api/users/{id}           - Get user profile
PUT    /api/users/{id}           - Update user profile
GET    /api/users/wishlist       - Get user's wishlist
POST   /api/users/wishlist       - Add to wishlist
DELETE /api/users/wishlist/{productId} - Remove from wishlist
```

---

## 🛡️ Security Features

- **JWT Authentication** - Secure token-based authentication
- **Password Encryption** - Industry-standard password hashing (bcrypt)
- **CORS Configuration** - Cross-origin request handling
- **Input Validation** - Server-side validation for all inputs
- **SQL Injection Prevention** - Parameterized queries via Entity Framework
- **Authorization Policies** - Role-based access control (Admin, User, Guest)
- **Rate Limiting** - Protection against brute-force attacks
- **Secure Headers** - Additional security headers for HTTP responses

---

## 🧪 Testing

The project includes comprehensive testing:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true

# Run specific test class
dotnet test --filter "ClassName=TestClassName"
```

### Test Categories
- Unit Tests - Individual component testing
- Integration Tests - API endpoint testing
- Database Tests - Data access layer testing

---

## 📊 Project Statistics

- **Primary Language**: C# (96.8%)
- **Secondary Language**: Python (3.2%)
- **Framework**: ASP.NET Core 6+
- **Database**: Entity Framework Core
- **Architecture**: Layered/N-Tier Architecture

---

## 🔄 Development Workflow

1. Create a feature branch: `git checkout -b feature/feature-name`
2. Make your changes and commit: `git commit -m 'Add feature'`
3. Push to your branch: `git push origin feature/feature-name`
4. Create a Pull Request with a detailed description
5. Code review and approval required before merge

### Code Quality Standards
- Follow C# naming conventions (PascalCase for public members)
- Write XML documentation for public methods
- Keep methods focused and under 30 lines
- Use meaningful variable names
- Add unit tests for new functionality

---

## 📋 Git Workflow

```bash
# Clone repository
git clone https://github.com/ahmedragab13579/E-Commerce.git

# Create feature branch
git checkout -b feature/your-feature

# Make changes and commit
git add .
git commit -m "Descriptive commit message"

# Push changes
git push origin feature/your-feature

# Create pull request on GitHub
```

---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

Please ensure your code follows the project's coding standards and includes appropriate documentation.

---

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## 📞 Support & Contact

For support, issues, or questions:
- Open an issue on GitHub: [Issues](https://github.com/ahmedragab13579/E-Commerce/issues)
- Contact: Ahmed Ragab
- Email: ahmedharidy@gmail.com
- Linked In: https://www.linkedin.com/in/ahmed-ragab-a7041b34b
- Work: https://www.forlanso.com/ar/bc/ahmd-ragab

---

## 🎯 Roadmap & Future Enhancements

- [ ] Payment gateway integration (Stripe, PayPal)
- [ ] Email notifications and order updates
- [ ] Advanced inventory management system
- [ ] Analytics and reporting dashboard
- [ ] Multi-language support (i18n)
- [ ] Mobile app (iOS/Android)
- [ ] Real-time notifications via SignalR
- [ ] Wishlist feature
- [ ] Product recommendations engine
- [ ] Customer loyalty program

---

## ✅ Checklist for New Developers

- [ ] Fork the repository
- [ ] Clone to local machine
- [ ] Install .NET 6 or higher
- [ ] Install SQL Server or use LocalDB
- [ ] Configure connection string in appsettings.json
- [ ] Run database migrations: `dotnet ef database update`
- [ ] Build the solution: `dotnet build`
- [ ] Run the application: `dotnet run`
- [ ] Run tests: `dotnet test`
- [ ] Start contributing!

---

**Last Updated**: June 2026  
**Version**: 1.0.0  
**Repository**: [ahmedragab13579/E-Commerce](https://github.com/ahmedragab13579/E-Commerce)
