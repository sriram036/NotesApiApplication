using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepo userRepo;

        public UserBusiness(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public UserEntity Register(RegisterModel register)
        {
            return userRepo.Register(register);
        }

        public UserEntity Login(int id)
        {
            return userRepo.Login(id);
        }

        public List<UserEntity> GetAllUsers() 
        { 
            return userRepo.GetAllUsers();
        }

        public bool CheckUser(string Email)
        {
            return userRepo.CheckUser(Email);
        }
    }
}
