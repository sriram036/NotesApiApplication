using BusinessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        public UsersController(IUserBusiness userBusiness, IBus bus)
        {
            this.userBusiness = userBusiness;
            this.bus = bus;
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

                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Mail Sent Successfully", Data = Email });
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

        [HttpPost]
        [Route("ResetPassword")]
        public ActionResult ResetPassword(ResetPasswordModel reset)
        {
            if (userBusiness.RestPassword(reset))
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
