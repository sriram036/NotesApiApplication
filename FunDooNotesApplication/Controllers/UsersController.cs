using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System.Collections.Generic;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        public UsersController(IUserBusiness userBusiness)
        {
            this.userBusiness = userBusiness;
            
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult Register(RegisterModel register)
        {
            var result = userBusiness.Register(register);
            if (result == null)
            {
                return Ok(new ResponseModel<UserEntity> { IsSuccess = false, Message = "User Failed", Data = result });
            }
            else
            {
                return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "Register Successfull", Data = result });
            }
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginModel loginModel)
        {
            var result = userBusiness.Login(loginModel);
            if (result == null)
            {
                return Ok(new ResponseModel<string> { IsSuccess = false, Message = "Login Failed", Data = result });
            }
            else
            {
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Login Successful", Data = result } );
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public List<UserEntity> GetAllUsers()
        {
            List<UserEntity> result = userBusiness.GetAllUsers();
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        [HttpGet]
        [Route("CheckUser")]
        public ActionResult CheckUser(string MailId)
        {
            bool IsUserExist = userBusiness.CheckUser(MailId);
            if (IsUserExist)
            {
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "User Found", Data = IsUserExist});
            }
            else
            {
                return Ok(new ResponseModel<bool> { IsSuccess = false, Message = "User Not Found", Data = IsUserExist });
            }
        }
    }
}
