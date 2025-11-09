using Application.Dtos.CartItem;
using Application.Dtos.Category;
using Application.Dtos.User;
using Application.Dtos.Login;
using Application.Dtos.Order;
using Application.Dtos.OrderItem;
using Application.Dtos.Payment;
using Application.Dtos.User;
using Application.Dtos.Product;
using Application.Dtos.User;
using AutoMapper;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Carts;

namespace E_Infrastructure.Services.Implementaions.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
          
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, AddUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            ///////
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<CartItem, AddCartItemDto>().ReverseMap();
            CreateMap<CartItem, UpdateCartItemDto>().ReverseMap();
            //////
            CreateMap<Cart, CartDto>().ReverseMap();
            //////
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, AddCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            //////
            CreateMap<Order, AddOrderDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, UpdateOrderStatusDto>().ReverseMap();
            //////
            CreateMap<OrderItem, AddOrderItemDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            ///////
            CreateMap<Product, AddProductDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            ///////
            CreateMap<Payment, AddPaymentDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
        }

    }
}
