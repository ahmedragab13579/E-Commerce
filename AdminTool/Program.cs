using AdminTool.Tools;
using Application.Dtos.User;
using Application.Services.InterFaces.Humans;
using E_Domain.Enums;
using ECommerce.Tools;
using EcommerceStoreApi; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    WriteError("Fatal: Connection string 'DefaultConnection' is missing.");
    return;
}

builder.Services.AddDatabase(connectionString);
builder.Services.AddApplicationServices();
builder.Services.AddSingleton<ConsoleCurrentUserService>();
builder.Services.AddScoped<ICurrentUserService>(sp => sp.GetRequiredService<ConsoleCurrentUserService>());
builder.Services.AddScoped<AdminSeeder>();
builder.Services.AddScoped<IsUserHasRole>();
builder.Services.AddScoped<AdminSeeder>();
builder.Services.AddScoped<IsUserHasRole>(); 

builder.Logging.ClearProviders().AddConsole();

using IHost host = builder.Build();


using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var seeder = services.GetRequiredService<AdminSeeder>();
    var authChecker = services.GetRequiredService<IsUserHasRole>();
    var config = services.GetRequiredService<IConfiguration>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
       
        if (args.Length < 7)
        {
            WriteError("Usage: dotnet run <YourSuperAdmin_User> <YourSuperAdmin_Pass> <MasterKey> <NewAdmin_FName> <NewAdmin_LName> <NewAdmin_User> <NewAdmin_Email> [Phone] [Password] [RoleID]");
            return;
        }

        logger.LogInformation($"Authenticating SuperAdmin '{args[0]}'...");
        await authChecker.HasPermission(args[0], args[1]);
        logger.LogInformation("SuperAdmin Authentication Successful.");

        var storedMasterKey = config["ToolSettings:MasterAdminKey"];
        if (string.IsNullOrEmpty(storedMasterKey))
        {
            WriteError("Fatal: MasterAdminKey is missing from appsettings.json.");
            return;
        }

        string enteredMasterKey = args[2];
        if (enteredMasterKey != storedMasterKey)
        {
            WriteError("Fatal: Invalid Master Key. Access denied.");
            return;
        }
        logger.LogInformation("MasterKey validation successful.");

        var newAdminDto = new AddNewAdminDto
        {
            FirstName = args[3],
            LastName = args[4],
            UserName = args[5],
            Email = args[6],
            Phone = args.Length > 7 ? args[7] : null,
            Password = args.Length > 8 ? args[8] : "Admin@123",
            RoleId = args.Length > 9 && int.TryParse(args[9], out var roleId) ? roleId : (int)Roles.Admin
        };

        await seeder.CreateAdminAsync(newAdminDto);
    }
    catch (Exception ex)
    {
        WriteError($"Operation Failed: {ex.Message}");
    }
}

void WriteError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(message);
    Console.ResetColor();
}