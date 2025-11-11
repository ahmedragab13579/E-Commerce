using Application.Dtos.User;
using Application.Services.InterFaces.Humans;
using E_Domain.Models;

namespace ECommerce.Tools
{
    
    public class ConsoleCurrentUserService : ICurrentUserService
    {
        private User? _currentUser;
        public void SetCurrentUser(User user)
        {
            _currentUser = user;
        }
        public int? GetCurrentUserId() => _currentUser?.Id;
        public string? GetUserName() => _currentUser?.UserName;
        public string? GetUserEmail() => _currentUser?.Email;
    }
}