using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface IUserBusiness
    {
        UserEntity Register(RegisterModel register);
        UserEntity Login(int id);
        List<UserEntity> GetAllUsers();
        bool CheckUser(string mail);
    }
}