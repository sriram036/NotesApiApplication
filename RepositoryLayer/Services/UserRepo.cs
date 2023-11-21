using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FunDooDBContext funDooDBContext;

        public UserRepo(FunDooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;
        }

        public UserEntity Register(RegisterModel register)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.FirstName = register.FirstName;
            userEntity.LastName = register.LastName;
            userEntity.Email = register.Email;
            userEntity.Password = register.Password;
            userEntity.CreatedAt = DateTime.Now;
            userEntity.ChangedAt = DateTime.Now;
            funDooDBContext.Users.Add(userEntity);
            funDooDBContext.SaveChanges();
            return userEntity;
        }

        public UserEntity Login(int id)
        {
            UserEntity userEntity = funDooDBContext.Find<UserEntity>(id);
            return userEntity;
        }

        public List<UserEntity> GetAllUsers() 
        {
            List<UserEntity> users = new List<UserEntity>();
            foreach (UserEntity user in funDooDBContext.Users)
            {
                users.Add(user);
            }
            return users;
        }

        public bool CheckUser(string mailId)
        {
            UserEntity isUserExist = funDooDBContext.Users.ToList().Find(user => user.Email == mailId);
            if (isUserExist == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
