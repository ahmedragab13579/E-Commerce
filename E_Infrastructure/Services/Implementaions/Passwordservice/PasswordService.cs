using Application.Services.InterFaces.PassWordServices;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.Services.Implementaions.Passwordservice
{
    public class PasswordService : IPasswordService
    {
        private readonly int _workFactor = 12;

        public PasswordService(IConfiguration configuration)
        {
            _workFactor = configuration.GetValue<int>("Security:BcryptWorkFactor", 12);
        }

        public string HashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, _workFactor);

        public bool VerifyPassword(string password, string hashedPassword)
            => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
