using Application.Dtos.Login;
using Application.Dtos.User;
using Application.Results;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.Login;
using E_Domain.Models;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EcommerceStoreApi.MainControllers.CustomerControllers.Login
{
    [Route("api/shop/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
     
        public LoginController(ILoginService _loginService)
        {
            this._loginService = _loginService;
          
        }

        /// <summary>
        /// Login To Your Account .
        /// </summary>
        /// <param name="loginDto">The User Login data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the User was Logined successfully.</response>
        /// <response code="404">If the User to Login is not found.</response>

        [HttpPost("Login")]
        [EnableRateLimiting("Login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            Result<string> result =  await _loginService.Login(loginDto);
            if(result.Success)
            {
                return ActionResultStatus.MapResult(new Result<string> { Data = result.Data, Code = "OK_WITH_TOKEN", Message = "login Was Success And Token Was Created", Success = true });
            }
            return ActionResultStatus.MapResult(result);

        }
    }
}
