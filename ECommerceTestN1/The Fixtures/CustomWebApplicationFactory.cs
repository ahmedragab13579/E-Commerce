using E_Domain.Models;               // 🛑 غيّر هذا: لـ namespace الموديلز
using E_Infrastructure.Data;         // 🛑 غيّر هذا: لـ namespace الـ DbContext
using ECommerceTestN1.The_Spy;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Linq;
using Testcontainers.RabbitMq;
using Xunit;

namespace ECommerceTestN1.The_Fixtures
{
    // 2. الكلاس ده بقى بيعمل "الاتنين"
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        // 3. هنجيب الكونتينر بتاع RabbitMQ ونخليه جزء من الـ Factory
        private readonly RabbitMqContainer _rabbitMqContainer;
        public CustomWebApplicationFactory()

// Add the NuGet package reference for Testcontainers.RabbitMq if it's missing
// Run the following command in the Package Manager Console or terminal:
// Install-Package Testcontainers -Version <latest_version>
        {
            // 4. بنبني الكونتينر في الـ Constructor الفاضي
            _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithUsername("guest")
                .WithPassword("guest")
                .Build();
        }

        // 5. دي الميثود السحرية اللي بتعدل Program.cs
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // --- الجزء الأول: تبديل قاعدة البيانات ---
                var dbContextDescriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<E_ApplicationDbContext>)); // 🛑 غيّر هذا: لاسم الـ DbContext

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<E_ApplicationDbContext>(options => // 🛑 غيّر هذا: لاسم الـ DbContext
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // --- الجزء الثاني: تبديل MassTransit ---
                var massTransitDescriptors = services.Where(d =>
                    d.ServiceType.Namespace?.Contains("MassTransit") == true).ToList();

                foreach (var descriptor in massTransitDescriptors)
                {
                    services.Remove(descriptor);
                }

                services.AddMassTransit(config =>
                {
                    config.AddConsumer<TestOrderPlacedConsumer>();

                    config.UsingRabbitMq((context, cfg) =>
                    {
                        // 6. نخليه يكلم الكونتينر اللي إحنا بنيناه
                        cfg.Host(_rabbitMqContainer.Hostname,
                                 _rabbitMqContainer.GetMappedPublicPort(5672),
                                 "/", h =>
                                 {
                                     h.Username("guest");
                                     h.Password("guest");
                                 });

                        cfg.ReceiveEndpoint("test-order-placed-queue", e =>
                        {
                            e.ConfigureConsumer<TestOrderPlacedConsumer>(context);
                        });
                    });
                });

                // --- الجزء الثالث: زراعة البيانات الوهمية (Seeding) ---
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<E_ApplicationDbContext>(); // 🛑 غيّر هذا: لاسم الـ DbContext
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                    try
                    {
                        db.Database.EnsureCreated();

                        // 🛑 (هنا كود زراعة البيانات بتاعك... زي المستخدم والمنتج)
                        // db.Users.Add(...);
                        // db.Products.Add(...);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "حدث خطأ أثناء زراعة البيانات الوهمية (Seeding).");
                    }
                }
            });
        }

        // 7. دي الميثود اللي هتشغل الكونتينر (من IAsyncLifetime)
        public async Task InitializeAsync()
        {
            await _rabbitMqContainer.StartAsync();
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Ensure that environment variables or additional args won't break host creation
            builder.UseEnvironment("Development");
            return base.CreateHost(builder);
        }

        // 8. دي الميثود اللي هتقفل الكونتينر (من IAsyncLifetime)
        public new async Task DisposeAsync()
        {
            await _rabbitMqContainer.StopAsync();
            await base.DisposeAsync();
        }
    }
}