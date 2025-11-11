using Application.DataReposatory.Interfaces.Humans;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.PassWordServices;
using E_Domain.Models;
using ECommerce.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminTool.Tools
{
    public class IsUserHasRole
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordService passwordService;
        public IsUserHasRole(IUserRepository userRepository, IPasswordService passwordService)
        {
            this.userRepository = userRepository;
            this.passwordService = passwordService;
        }


        public async Task<bool> HasPermission(string username,string password)
        {
            User user = await userRepository.GetByUserNameAsync(username);
            if (user == null)
                throw new Exception("User not found.");
            if (!passwordService.VerifyPassword(password,user.PasswordHash))
                throw new Exception("Invalid username or  password.");
            if (user.RoleId != (int)E_Domain.Enums.Roles.SuperAdmin)
                throw new Exception("User does not have SuperAdmin role.");
            ConsoleCurrentUserService ConsoleCurrentUserService = new ConsoleCurrentUserService();
            ConsoleCurrentUserService.SetCurrentUser(user);
            return true;
        }   
    }
}
