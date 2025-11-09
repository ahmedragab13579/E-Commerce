using E_Infrastructure.MiddleWare.ErrorService;
using E_Infrastructure.MiddleWare.IsBlockedUser;
using E_Infrastructure.MiddleWare.Order_Amount_Service;
using EcommerceStoreApi;
using MassTransit;
using Serilog;

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseMiddleware<OrderQuantityValidationMiddleware>();
app.UseMiddleware<AccountStatusMiddleware>();

app.MapControllers();

try
{
    Log.Information("Starting web host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}