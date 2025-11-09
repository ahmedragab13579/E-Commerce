using MassTransit;
using NotificationService.Consumers;
using NotificationService.Service.Interfaces;
using SharedMessages;
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => 
    {
        services.Configure<EmailSettings>(hostContext.Configuration.GetSection("EmailSettings"));

        services.AddScoped<IEmailService, EmailService>();

        services.AddMassTransit(config => {
            config.AddConsumer<Consumer>();
            config.UsingRabbitMq((context, cfg) => {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ReceiveEndpoint("order-placed-queue", e => {
                    e.ConfigureConsumer<Consumer>(context);
                });
            });
        });
    })
    .Build();

await host.RunAsync();