using BusinessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        private readonly IBus bus;
        private readonly ILogger<UsersController> logger;
        public UsersController(IUserBusiness userBusiness, IBus bus, ILogger<UsersController> logger)
        {
            this.userBusiness = userBusiness;
            this.bus = bus;
            this.logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult Register(RegisterModel register)
        {
            var result = userBusiness.Register(register);
            if (result == null)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User Register Failed", Data = "Email Already Exist" });
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
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Login Failed", Data = result });
            }
            else
            {
                logger.LogInformation("Login Successful");
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Login Successful", Data = result } );
            }
        }

        [HttpPost]
        [Route("LoginToGetUser")]
        public ActionResult LoginToReturnUserEntity(LoginModel loginModel)
        {
            UserEntity userEntity = userBusiness.LoginToReturnUserEntity(loginModel);
            if(userEntity != null)
            {
                HttpContext.Session.SetInt32("UserId",userEntity.UserId);
                return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "Login Successful", Data = userEntity });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Login Failed", Data = "EmailId or Password is wrong" });
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
        public bool CheckUser(string MailId)
        {
            bool IsUserExist = userBusiness.CheckUser(MailId);
            if (IsUserExist)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            try
            {
                if (userBusiness.CheckUser(Email))
                {
                    Send send = new Send();
                    ForgotPasswordModel forgotPasswordModel = userBusiness.ForgotPassword(Email);
                    send.SendMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                    Uri uri = new Uri("rabbitmq://localhost/FunDooNotesEmailQueue");
                    var endPoint = await bus.GetSendEndpoint(uri);

                    await endPoint.Send(forgotPasswordModel);

                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Mail Sent Successfully", Data = forgotPasswordModel.Token });
                }
                else
                {
                    return BadRequest(new ResponseModel<string>() { IsSuccess = false, Message = "Email Does Not Exist", Data = Email });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPost]
        [Route("ResetPassword")]
        public ActionResult ResetPassword(ResetPasswordModel reset)
        {
            string Email = User.FindFirst("Email").Value;
            if (userBusiness.ResetPassword(Email, reset))
            {
                return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Password Changed", Data = true });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccess = false, Message = "Password Not Changed", Data = false });
            }
        }
    }
}
