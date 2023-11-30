using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
        UserEntity Register(RegisterModel register);
        string Login(LoginModel loginModel);
        List<UserEntity> GetAllUsers();
        bool CheckUser(string mail);
        ForgotPasswordModel ForgotPassword(string Email);
        bool ResetPassword(string Email, ResetPasswordModel reset);
        UserEntity LoginToReturnUserEntity(LoginModel loginModel);
    }
}