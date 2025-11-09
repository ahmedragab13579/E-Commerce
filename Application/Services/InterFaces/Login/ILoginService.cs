using Application.Dtos.Login;
using Application.Dtos.User;
using Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces.Login
{
    public interface ILoginService
    {
        Task<Result<string>> Login(LoginDto login);
    }
}
