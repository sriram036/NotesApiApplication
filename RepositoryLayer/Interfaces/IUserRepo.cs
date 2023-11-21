using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
        UserEntity Register(RegisterModel register);
        UserEntity Login(int id);
        List<UserEntity> GetAllUsers();
        bool CheckUser(string mail);
    }
}