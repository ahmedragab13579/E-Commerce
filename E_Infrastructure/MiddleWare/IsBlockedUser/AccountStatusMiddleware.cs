using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Services.InterFaces.Humans;
using E_Infrastructure.DataRepository.Implementaion.UnitOfWork;
using E_Infrastructure.Services.Implementaions.Humans;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.MiddleWare.IsBlockedUser
{
    public class AccountStatusMiddleware
    {

        private readonly RequestDelegate _next;
        public AccountStatusMiddleware(RequestDelegate _next)
        {
            this._next = _next;
        }


        public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService,  IUnitOfWork unitOfWork)
        {
            var userId = currentUserService.GetCurrentUserId();
            if (userId == null)
            {
                await _next(context);
                return;
            }
            if (userId != 0)
            {
                var user = await unitOfWork.Users.GetByIdAsync(userId.Value);

                if (user != null && user.IsBlocked)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access denied: Your account is blocked.");
                    return;
                }
                if (user != null && user.IsDeleted)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden; 
                    await context.Response.WriteAsync("Access denied: Your account has been deleted.");
                    return; 
                }
            }

            await _next(context);
        }
    }
}
