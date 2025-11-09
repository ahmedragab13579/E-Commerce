using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.Login;
using Application.Dtos.User;
using Application.Results;
using Application.Services.InterFaces.Mapping;
using Application.Services.InterFaces.PassWordServices;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces.Humans
{
    public interface IUserService
    {
        Task<Result<int>> Add(AddUserDto NewUser);
        Task<Result<UserDto>> Login(LoginDto login);
        Task<Result<int>> ChangePassword(string OldPassword, string NewPassword);
        Task<Result<List<UserDto>>> GetMustActiveUsers(int count = 10);


        Task<Result<int>> ForgotPassword(string Email, string NewPassword);
      
        Task<Result<UserDto>> GetById(int id);
        Task<Result<int>> Update(UpdateUserDto entity);

        Task<Result<List<UserDto>>> GetAll();

        Task<Result<UserDto>> GetUserWithOrders(int id);

        Task<Result<List<UserDto>>> GetAllActiveUsers();

        Task<Result<int>> BlockUser(int id, string reason = "No reason provided.");


        Task<Result<int>> ActivateUser(int id);


        Task<Result<int>> SoftDeleteUser(int id, string reason = "No reason provided.");




        Task<Result<int>> RestoreUser(int id);
      




    }
}
