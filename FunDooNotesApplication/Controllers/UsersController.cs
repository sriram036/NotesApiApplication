using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("Login")]
        public ActionResult Login(int id)
        {
            var result = userBusiness.Login(id);
            if (result == null)
            {
                return Ok(new ResponseModel<UserEntity> { IsSuccess = false, Message = "User Not Found", Data = result });
            }
            else
            {
                return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "User Found", Data = result } );
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
        public string CheckUser(string MailId)
        {
            bool IsUserExist = userBusiness.CheckUser(MailId);
            if (IsUserExist)
            {
                return "User Found.";
            }
            else
            {
                return "User Not Found.";
            }
        }
    }
}
