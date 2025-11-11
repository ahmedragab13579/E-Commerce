using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.User;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.Registration;
using E_Domain.Enums; 
using Microsoft.Extensions.Logging;

namespace ECommerce.Tools
{
    public class AdminSeeder
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AdminSeeder> _logger;

        public AdminSeeder(
IUserService _userService,
IUnitOfWork unitOfWork,
            ILogger<AdminSeeder> logger)
        {
            this._userService = _userService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task CreateAdminAsync(AddNewAdminDto adminDto)
        {
            Console.WriteLine("--------------------------------------------------");
            _logger.LogInformation("Starting Admin Creation Process...");

            try
            {
                var roleExists = await _unitOfWork.Role.GetByIdAsync(adminDto.RoleId);

                if (roleExists == null)
                {
                    WriteError($"Error: Admin Role (ID: {adminDto.RoleId}) not found in DB. Please seed roles first.");
                    return;
                }

               
               
                _logger.LogInformation($"Creating user: {adminDto.Email}...");
                var result = await _userService.Add(adminDto);

                if (result.Success)
                {
                    WriteSuccess($"SUCCESS: Admin '{adminDto.FirstName} {adminDto.LastName}' created successfully.");
                }
                else
                {
                    WriteError($"FAILED: Could not create admin. Reason: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                WriteError($"CRITICAL ERROR: {ex.Message}");
                _logger.LogError(ex, "Error occurred during admin seeding.");
            }
            finally
            {
                Console.WriteLine("--------------------------------------------------");
            }
        }

        private void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}