using Application.Dtos.User;
using Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces.Registration
{
    public interface IRegistrationService
    {
        Task<Result<int>> Registration(AddUserDto NewUser);
    }
}
