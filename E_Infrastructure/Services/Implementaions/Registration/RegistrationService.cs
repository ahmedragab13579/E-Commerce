using Application.Dtos.User;
using Application.Results;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.Registration;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.Services.Implementaions.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserService _UserService;
        public RegistrationService(IUserService _UserService)
        {
            this._UserService = _UserService;

        }
        public async Task<Result<int>> Registration(AddUserDto NewUser)
        {
            var result = await _UserService.Add(NewUser);
            if (result.Success)
            {
                return Result<int>.Ok(result.Data, "Wellcome Your Account Become Avilable");
            }
            return result;
        }
    }
}
