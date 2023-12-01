using BusinessLayer.Interfaces;
using GreenPipes.Caching;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
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
        private readonly IDistributedCache cache;
        public UsersController(IUserBusiness userBusiness, IBus bus, ILogger<UsersController> logger, IDistributedCache cache)
        {
            this.userBusiness = userBusiness;
            this.bus = bus;
            this.logger = logger;
            this.cache = cache;
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult Register(RegisterModel register)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginModel loginModel)
        {
            try
            {
                var result = userBusiness.Login(loginModel);
                if (result == null)
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Login Failed", Data = result });
                }
                else
                {
                    logger.LogInformation("Login Successful");
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Login Successful", Data = result });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("LoginToGetUser")]
        public ActionResult LoginToReturnUserEntity(LoginModel loginModel)
        {
            try
            {
                UserEntity userEntity = userBusiness.LoginToReturnUserEntity(loginModel);
                if (userEntity != null)
                {
                    HttpContext.Session.SetInt32("UserId", userEntity.UserId);
                    return Ok(new ResponseModel<UserEntity> { IsSuccess = true, Message = "Login Successful", Data = userEntity });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Login Failed", Data = "EmailId or Password is wrong" });
                }
            }
             catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public List<UserEntity> GetAllUsers()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<List<UserEntity>> GetAllUsersWithCache(bool enableCache)
        {
            try
            {
                if (!enableCache)
                {
                    List<UserEntity> result = userBusiness.GetAllUsers();
                    return result;
                }
                string cacheKey = "sri@gmail.com";

                // Trying to get data from the Redis cache
                byte[] cachedData = await cache.GetAsync(cacheKey);
                List<UserEntity> Users = new List<UserEntity>();
                if (cachedData != null)
                {
                    // If the data is found in the cache, encode and deserialize cached data.
                    string cachedDataString = Encoding.UTF8.GetString(cachedData);
                    Users = JsonSerializer.Deserialize<List<UserEntity>>(cachedDataString);
                }
                else
                {
                    // If the data is not found in the cache, then fetch data from database
                    Users = userBusiness.GetAllUsers();

                    // Serializing the data
                    string cachedDataString = JsonSerializer.Serialize(Users);
                    var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                    // Setting up the cache options
                    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                    // Add the data into the cache
                    await cache.SetAsync(cacheKey, dataToCache, options);
                }
                return Users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("CheckUser")]
        public bool CheckUser(string MailId)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
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
            try
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
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetUsersBasedOnLabel")]
        public List<UserEntity> GetUsersBasedOnLabel(string label)
        {
            try
            {
                List<UserEntity> Users = userBusiness.GetUsersBasedOnLabel(label);
                return Users;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
