
namespace EcommerceStoreApi
{
    using Application.DataReposatory.Interfaces;
    using Application.DataReposatory.Interfaces.Carts;
    using Application.DataReposatory.Interfaces.Humans;
    using Application.DataReposatory.Interfaces.Orders;
    using Application.DataReposatory.Interfaces.UnitOfWork;
    using Application.Services.InterFaces;
    using Application.Services.InterFaces.CartSession;
    using Application.Services.InterFaces.Humans;
    using Application.Services.InterFaces.Login;
    using Application.Services.InterFaces.Mapping;
    using Application.Services.InterFaces.Ordres;
    using Application.Services.InterFaces.PassWordServices;
    using Application.Services.InterFaces.Registration;
    using E_Domain.Enums;
    using E_Infrastructure.Data;
    using E_Infrastructure.DataRepository.Implementaion;
    using E_Infrastructure.DataRepository.Implementaion.Carts;
    using E_Infrastructure.DataRepository.Implementaion.Humans;
    using E_Infrastructure.DataRepository.Implementaion.Orders;
    using E_Infrastructure.DataRepository.Implementaion.UnitOfWork;
    using E_Infrastructure.Services.Implementaions;
    using E_Infrastructure.Services.Implementaions.CardSession;
    using E_Infrastructure.Services.Implementaions.Humans;
    using E_Infrastructure.Services.Implementaions.Login;
    using E_Infrastructure.Services.Implementaions.Mapping;
    using E_Infrastructure.Services.Implementaions.Orders;
    using E_Infrastructure.Services.Implementaions.Passwordservice;
    using E_Infrastructure.Services.Implementaions.Registration;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Serilog.Events;
    using Serilog.Sinks.MSSqlServer;
    using StackExchange.Redis;
    using System.Text;
    using System.Threading.RateLimiting;

    public static class RegistrationExtensions
    {
        public static ConfigureHostBuilder AddSerilog(this ConfigureHostBuilder host, IConfiguration config, string connectionString)
        {
            var loginColumnOptions = new ColumnOptions
            {
                AdditionalColumns = new List<SqlColumn>
                {
                    new SqlColumn { ColumnName = "UserID", DataType = System.Data.SqlDbType.Int, AllowNull = true },
                    new SqlColumn { ColumnName = "UserNameAttempt", DataType = System.Data.SqlDbType.NVarChar, DataLength = 255 },
                    new SqlColumn { ColumnName = "LoginStatus", DataType = System.Data.SqlDbType.Bit },
                    new SqlColumn { ColumnName = "IPAddress", DataType = System.Data.SqlDbType.NVarChar, DataLength = 45, AllowNull = true },
                    new SqlColumn { ColumnName = "UserAgent", DataType = System.Data.SqlDbType.NVarChar, DataLength = 500, AllowNull = true }
                }
            };
            loginColumnOptions.Store.Remove(StandardColumn.Message);
            loginColumnOptions.Store.Remove(StandardColumn.MessageTemplate);
            loginColumnOptions.Store.Remove(StandardColumn.Level);
            loginColumnOptions.Store.Remove(StandardColumn.Exception);
            loginColumnOptions.Store.Remove(StandardColumn.Properties);
            loginColumnOptions.TimeStamp.ColumnName = "LoginTime";

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "E-Commerce-App")
                .WriteTo.Console()
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("LoginStatus"))
                    .WriteTo.MSSqlServer(
                        connectionString: connectionString,
                        sinkOptions: new MSSqlServerSinkOptions { TableName = "LoginAttemptsLog", AutoCreateSqlTable = true },
                        columnOptions: loginColumnOptions
                    )
                )
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Warning)
                    .WriteTo.MSSqlServer(
                        connectionString: connectionString,
                        sinkOptions: new MSSqlServerSinkOptions { TableName = "ErrorLogs", AutoCreateSqlTable = true }
                    )
                )
                .CreateLogger();

            host.UseSerilog();
            return host;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<AuditInterceptor>();
            services.AddDbContext<E_ApplicationDbContext>((sp, options) =>
                options
                    .UseSqlServer(connectionString)
                    .AddInterceptors(sp.GetRequiredService<AuditInterceptor>())
            );
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>(); 
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IMappingServices, MappingService>();
            services.AddScoped<ICartSessionService, CartSessionService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRecommendationService, RecommendationService>();
            services.AddScoped<IWishListService, WishListService>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
                };
            });

            services.AddAuthorization(options =>
            {

                options.AddPolicy("Customer", policy =>
                    policy.RequireRole(Roles.Customer.ToString()));

                options.AddPolicy("Admin", policy =>
                    policy.RequireRole(Roles.Admin.ToString()));
            });

            return services;
        }
       

        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers().AddMvcOptions(options =>
            {
                options.MaxModelValidationErrors = 50;
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    _ => "The field is required.");
            });

            services.AddHttpContextAccessor();

            services.AddHttpClient("RecommendationService", client =>
            {
                client.BaseAddress = new Uri(config["RecommendationService:Url"] ?? "http://127.0.0.1:8000");
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1)
                        });
                });

                options.AddPolicy("Login", context =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromSeconds(30)
                        });
                });

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken: token);
                };
            });

            return services;
        }

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