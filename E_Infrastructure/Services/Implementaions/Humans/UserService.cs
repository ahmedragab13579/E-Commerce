using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.Login;
using Application.Dtos.User;
using Application.Results;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.Mapping;
using Application.Services.InterFaces.PassWordServices;
using E_Domain.Models;



namespace E_Infrastructure.Services.Implementaions.Humans
{
    public class UserService : IUserService
    {
        private readonly IMappingServices _mappingServices;
        private readonly IPasswordService _passwordService;
        private readonly ICurrentUserService currentUserService;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IPasswordService passwordService, IMappingServices mappingServices/*, ILogger<UserService> logger*/)
        {
            _mappingServices = mappingServices;
            _passwordService = passwordService;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Add(AddUserDto Perosn)
        {
            if (Perosn == null)
            {
                return Result<int>.Fail("NULL_REFERENCE", "User Object Is Null");
            }

            if (await unitOfWork.Users.IsUserNameExist(Perosn.UserName))
            {
                return Result<int>.Fail("DUPLICATE_USER_NAME", "User Name Is Already Exist");
            }
            if (await unitOfWork.Users.IsEmailExist(Perosn.Email))
            {
                return Result<int>.Fail("DUPLICATE_EMAIL", "Email Is Already Exist");
            }
            try
            {
                var NewUser = new User(Perosn.FirstName, Perosn.LastName, Perosn.UserName, Perosn.Email, Perosn.Phone, _passwordService.HashPassword(Perosn.Password));
                await unitOfWork.Users.AddAsync(NewUser);
                int success = await unitOfWork.CompleteTask();
                if (success > 0)
                {
                    return Result<int>.Ok(NewUser.Id, "User Added Successfully"); 
                }
                return Result<int>.Fail("SAVE_FAILED", "Could not save the user.");
            }
            catch (ArgumentException ex)
            {
                return Result<int>.Fail("INVALID_DATA", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<int>> Update(UpdateUserDto entity)
        {
            if (entity == null)
            {
                return Result<int>.Fail("NULL_REFERENCE", "User Object Is Null");
            }
            int? Userid = currentUserService.GetCurrentUserId();
            if (Userid.Value <= 0|| Userid==null)
                return Result<int>.Fail("UNAUTHOURISED", "User Not Login System");
            User user = await unitOfWork.Users.GetByIdAsync(Userid.Value);
            if (user == null)
            {
                return Result<int>.Fail("NOT_FOUND", "User Not Found Or Wrong ID");
            }

            if (user.UserName != entity.UserName && await unitOfWork.Users.IsUserNameExist(entity.UserName))
            {
                return Result<int>.Fail("DUPLICATE_USER_NAME", "User Name Is Already Exist");
            }
            if (user.Email != entity.Email && await unitOfWork.Users.IsEmailExist(entity.Email))
            {
                return Result<int>.Fail("DUPLICATE_EMAIL", "Email Is Already Exist");
            }

            try
            {
                user.UpdateProfile(entity.FirstName, entity.LastName, entity.UserName, entity.Email, entity.Phone);
                unitOfWork.Users.Update(user);
                int success = await unitOfWork.CompleteTask();
                if (success > 0)
                {
                    return Result<int>.Ok(success, "User Updated Successfully");
                }
                return Result<int>.Fail("SAVE_FAILED", "Could not update the user.");
            }
            catch (ArgumentException ex)
            {
                return Result<int>.Fail("INVALID_DATA", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<List<UserDto>>> GetMustActiveUsers(int count = 10)
        {
            var users = await unitOfWork.Users.GetMostActiveCustomersAsync(count);
            if (users == null || !users.Any())
            {
                return Result<List<UserDto>>.Fail("NO_ACTIVE_USERS_FOUND", "No Active Users Found");
            }
            var userDtos = _mappingServices.Map<List<User>, List<UserDto>>(users.ToList());
            return Result<List<UserDto>>.Ok(userDtos, "Active Users Retrieved Successfully");
        }

        public async Task<Result<int>> ChangePassword(string OldPassword, string NewPassword)
        {
            int? UserId = currentUserService.GetCurrentUserId();
            if (UserId == null)
            {
                return Result<int>.Fail("UNAUTHORIZED", "User Is Not Logged In");
            }
            var user = await unitOfWork.Users.GetByIdAsync(UserId.Value);
            if (user == null)
            {
                return Result<int>.Fail("NOT_FOUND", "User Not Found Or Wrong ID");
            }
            bool isOldPasswordValid = _passwordService.VerifyPassword(OldPassword, user.PasswordHash);
            if (!isOldPasswordValid)
            {
                return Result<int>.Fail("INVALID_OLD_PASSWORD", "Old Password Is Incorrect");
            }

            try
            {
                string newHashedPassword = _passwordService.HashPassword(NewPassword);
                user.ChangePassword(newHashedPassword); 
                unitOfWork.Users.Update(user);
                int success = await unitOfWork.CompleteTask();
                if (success > 0)
                {
                    return Result<int>.Ok(success, "Password Changed Successfully");
                }
                return Result<int>.Fail("SAVE_FAILED", "Could not change the password.");
            }
            catch (ArgumentException ex)
            {
                return Result<int>.Fail("INVALID_DATA", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<int>> ForgotPassword(string Email, string NewPassword)
        {
            var user = await unitOfWork.Users.GetUserByEmail(Email);
            if (user == null)
            {
                return Result<int>.Fail("NOT_FOUND", "User Not Found Or Wrong Email");
            }

            try
            {
                string newHashedPassword = _passwordService.HashPassword(NewPassword);
                user.ChangePassword(newHashedPassword); 
                unitOfWork.Users.Update(user);
                int success = await unitOfWork.CompleteTask();
                if (success > 0)
                {
                    return Result<int>.Ok(success, "Password Reset Successfully");
                }
                return Result<int>.Fail("SAVE_FAILED", "Could not reset the password.");
            }
            catch (ArgumentException ex)
            {
                return Result<int>.Fail("INVALID_DATA", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<UserDto>> GetById(int id)
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                return Result<UserDto>.Ok(_mappingServices.Map<User, UserDto>(user), "User Was Found");
            }
            return Result<UserDto>.Fail("NOT_FOUND", "User Not Found Or Wrong ID");
        }

        public async Task<Result<UserDto>> Login(LoginDto login)
        {
            var user = await unitOfWork.Users.GetByUserNameAsync(login.UserName);
            if (user == null)
            {
                return Result<UserDto>.Fail("INVALID_CREDENTIALS", "Invalid Username or Password");
            }
            if (user.IsBlocked)
            {
                return Result<UserDto>.Fail("ACCOUNT_BLOCKED", "Your account is blocked.");
            }
            if (user.IsDeleted)
            {
                return Result<UserDto>.Fail("ACCOUNT_DELETED", "Your account has been deleted.");
            }

            bool isPasswordValid = _passwordService.VerifyPassword(login.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Result<UserDto>.Fail("INVALID_CREDENTIALS", "Invalid Username or Password");
            }
            return Result<UserDto>.Ok(_mappingServices.Map<User, UserDto>(user), "Login Successful");
        }

        public async Task<Result<List<UserDto>>> GetAll()
        {
            var users = await unitOfWork.Users.GetAllAsync();
            if (users == null || !users.Any())
            {
                return Result<List<UserDto>>.Fail("NO_USERS_FOUND", "No Users Found");
            }
            var userDtos = _mappingServices.Map<List<User>, List<UserDto>>(users.ToList());
            return Result<List<UserDto>>.Ok(userDtos, "Users Retrieved Successfully");
        }

        public async Task<Result<UserDto>> GetUserWithOrders(int id)
        {
            var user = await unitOfWork.Users.GetUserWithOrders(id);
            if (user == null)
            {
                return Result<UserDto>.Fail("NOT_FOUND", "User Not Found Or Wrong ID");
            }
            var userDto = _mappingServices.Map<User, UserDto>(user);
            return Result<UserDto>.Ok(userDto, "User With Orders Retrieved Successfully");
        }

        public async Task<Result<List<UserDto>>> GetAllActiveUsers()
        {
            var users = await unitOfWork.Users.GetAllActiveUsers();
            if (users == null || !users.Any())
            {
                return Result<List<UserDto>>.Fail("NO_ACTIVE_USERS_FOUND", "No Active Users Found");
            }
            var userDtos = _mappingServices.Map<List<User>, List<UserDto>>(users.ToList());
            return Result<List<UserDto>>.Ok(userDtos, "Active Users Retrieved Successfully");
        }

        public async Task<Result<int>> BlockUser(int id, string reason = "No reason provided.") 
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return Result<int>.Fail("NOT_FOUND", "User Not Found Or Wrong ID");
            }
            if (user.IsBlocked)
            {
                return Result<int>.Fail("ALREADY_BLOCKED", "User Is Already Blocked");
            }
            try
            {
                user.Block(reason); 
                unitOfWork.Users.Update(user);
                int success = await unitOfWork.CompleteTask();
                if (success > 0)
                {
                    return Result<int>.Ok(success, "User Blocked Successfully");
                }
                return Result<int>.Fail("SAVE_FAILED", "Could not block the user.");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<int>> ActivateUser(int id)
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return Result<int>.Fail("NOT_FOUND", "User Not Found Or Wrong ID");
            }
            if (!user.IsBlocked)
            {
                return Result<int>.Fail("ALREADY_ACTIVE", "User Is Already Active");
            }
            try
            {
                user.Unblock(); 
                unitOfWork.Users.Update(user);
                int success = await unitOfWork.CompleteTask();
                if (success > 0)
                {
                    return Result<int>.Ok(success, "User Activated Successfully");
                }
                return Result<int>.Fail("SAVE_FAILED", "Could not activate the user.");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<int>> SoftDeleteUser(int id, string reason = "No reason provided.")
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return Result<int>.Fail("NOT_FOUND", "User Not Found Or Wrong ID");
            }
            if (user.IsDeleted)
            {
                return Result<int>.Fail("ALREADY_DELETED", "User Is Already Deleted");
            }
            try
            {
                user.SoftDelete(reason); 
                unitOfWork.Users.Update(user);
                int success = await unitOfWork.CompleteTask();
                if (success > 0)
                {
                    return Result<int>.Ok(success, "User Soft Deleted Successfully");
                }
                return Result<int>.Fail("SAVE_FAILED", "Could not soft delete the user Because He Was Connected With Another Data.");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<int>> RestoreUser(int id)
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return Result<int>.Fail("NOT_FOUND", "User Not Found Or Wrong ID");
            }
            if (!user.IsDeleted)
            {
                return Result<int>.Fail("NOT_DELETED", "User Is Not Deleted");
            }
            try
            {
                user.Restore(); 
                unitOfWork.Users.Update(user);
                int success = await unitOfWork.CompleteTask();
                if (success > 0)
                {
                    return Result<int>.Ok(success, "User Restored Successfully");
                }
                return Result<int>.Fail("SAVE_FAILED", "Could not restore the user.");
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}