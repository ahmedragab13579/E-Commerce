using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces.Humans
{
    public interface ICurrentUserService
    {
        int? GetCurrentUserId();
        string? GetUserName();
        string? GetUserEmail();


    }
}
