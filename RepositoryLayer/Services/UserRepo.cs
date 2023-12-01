using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FunDooDBContext funDooDBContext;
        private readonly IConfiguration _config;

        public UserRepo(FunDooDBContext funDooDBContext, IConfiguration config)
        {
            this.funDooDBContext = funDooDBContext;
            _config = config;
        }

        public UserEntity Register(RegisterModel register)
        {
            UserEntity UserEmail = funDooDBContext.Users.ToList().Find(user => user.Email == register.Email);
            UserEntity userEntity = new UserEntity();
            if (UserEmail == null)
            {
                userEntity.FirstName = register.FirstName;
                userEntity.LastName = register.LastName;
                userEntity.Email = register.Email;
                userEntity.Password = EncodePassword(register.Password);
                userEntity.CreatedAt = DateTime.Now;
                userEntity.ChangedAt = DateTime.Now;
                funDooDBContext.Users.Add(userEntity);
                funDooDBContext.SaveChanges();
                return userEntity;
            }
            else 
            {
                return null;
            }
            
        }

        public string Login(LoginModel loginModel)
        {
            UserEntity userEntity = funDooDBContext.Users.ToList().Find(x => x.Email == loginModel.Email && x.Password == EncodePassword(loginModel.Password));
            if(userEntity == null)
            {
                return null;
            }
            else
            {
                string token = GenerateToken(userEntity.Email, userEntity.UserId);
                return token;
            }
        }

        public string GenerateToken(string Email, int UserId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",Email),
                new Claim("UserId",UserId.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issue"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string EncodePassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
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

        public ForgotPasswordModel ForgotPassword(string Email)
        {
            UserEntity User = funDooDBContext.Users.ToList().Find(user => user.Email == Email );
            ForgotPasswordModel forgotPassword = new ForgotPasswordModel();
            forgotPassword.Email = User.Email;
            forgotPassword.UserId = User.UserId;
            forgotPassword.Token = GenerateToken(User.Email, User.UserId);
            return forgotPassword;
        }

        public bool ResetPassword(string Email ,ResetPasswordModel resetPasswordModel)
        {
            UserEntity User = funDooDBContext.Users.ToList().Find(user => user.Email == Email);
            
            if(CheckUser(User.Email))
            {
                User.Password = EncodePassword(resetPasswordModel.ConfirmPassword);
                User.ChangedAt = DateTime.Now; 
                funDooDBContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserEntity LoginToReturnUserEntity(LoginModel loginModel)
        {
            UserEntity userEntity = funDooDBContext.Users.ToList().Find(x => x.Email == loginModel.Email && x.Password == EncodePassword(loginModel.Password));
            if (userEntity == null)
            {
                return null;
            }
            else
            {
                return userEntity;
            }
        }

        public List<UserEntity> GetUsersBasedOnLabel(string label)
        {
            List<int> userIds = funDooDBContext.Labels.ToList().FindAll(x => x.LabelName == label).Select(id => id.UserId).ToList();
            List<UserEntity> Users = new List<UserEntity>();
            foreach (int id in userIds)
            {
                UserEntity user = funDooDBContext.Users.FirstOrDefault(user => user.UserId == id);
                Users.Add(user);
            }
            return Users;
        }
    }
}
