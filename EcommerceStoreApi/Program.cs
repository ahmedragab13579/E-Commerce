using E_Infrastructure;
using E_Infrastructure.MiddleWare.ErrorService;
using E_Infrastructure.MiddleWare.IsBlockedUser;
using E_Infrastructure.MiddleWare.Order_Amount_Service;
using EcommerceStoreApi; 
using MassTransit;
using Microsoft.OpenApi.Models; 
using Serilog;
using Application.Services.InterFaces.Humans; 

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Host.AddSerilog(builder.Configuration, connectionString);

builder.Services
    .AddDatabase(connectionString)
    .AddApplicationServices()
    .AddSecurity(builder.Configuration)
    .AddWebServices(builder.Configuration)
    .AddSwagger(); 
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, E_Infrastructure.Services.Implementaions.Humans.CurrentUserService>();


builder.Services.AddMassTransit(configure =>
{
    configure.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Run();

namespace EcommerceStoreApi
{
    public static class RegistrationExtensions
    {
     
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => 
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token (e.g., 'Bearer eyJhbGciOi...')"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            return services;
        }
    }
}