using Application.Dtos.User;
using Application.Results;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.Registration;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace EcommerceStoreApi.MainControllers.CustomerControllers.Register
{
    [Route("api/shop/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
         private readonly IRegistrationService _services;
        public RegistrationController(IRegistrationService _services)
        {
            this._services = _services;
            
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="NewUser">The user data for registration.</param>
        /// <returns>An action result containing the newly created user's ID.</returns>
        /// <response code="200">Returns the newly created user's ID.</response>
        /// <response code="400">If the user data is invalid (e.g., duplicate email).</response>
        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] AddUserDto NewUser)
        {
            Result<int> Result = await _services.Registration(NewUser);
            return ActionResultStatus.MapResult(Result);
        }

    }
}
