using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface IUserBusiness
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