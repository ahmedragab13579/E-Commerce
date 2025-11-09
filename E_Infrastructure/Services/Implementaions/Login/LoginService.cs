using Application.Dtos.Login;
using Application.Dtos.User;
using Application.Results;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.Login;
using E_Domain.Models;
using E_Infrastructure.Services.Implementaions.Mapping;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.Services.Implementaions.Login
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LoginService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        public LoginService(  ILogger<LoginService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor, IUserService _userService)
        {
            _config = config;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
           this. _userService = _userService;
        }

        public async Task<Result<string>> Login(LoginDto login)
        {
            Result<UserDto> result = await _userService.Login(login);

            string ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "N/A";
            string userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "N/A";


            int? UserId = result.Success ? result.Data.Id : (int?)null;
            _logger.LogInformation(
    "Login Attempt: {UserNameAttempt} | Status: {LoginStatus} | UserID: {UserID} | IP: {IPAddress} | UserAgent: {UserAgent}",
    login.UserName,
    result.Success,
    UserId,
    ipAddress,
    userAgent
);

           
            if (!result.Success)
            {
                return Result<string>.Fail("LOGIN_FAILED", result.Message);
            }

            UserId = result.Data.Id;

            return Result<string>.Ok( GenerateJwtToken(result.Data),"Token Was Maked");
        }

        private string GenerateJwtToken(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role,user.Role.Name), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);



        }
    }                                       
}