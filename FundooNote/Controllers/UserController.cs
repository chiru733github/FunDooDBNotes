using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces;
using Model_Layer.Models;
using RepositoryLayer.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using RepositoryLayer.Context;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        private readonly IBus _bus;
        private readonly IDistributedCache _cache;
        public UserController(IUserBusiness userBusiness, IBus bus ,IDistributedCache cache)
        {
            this.userBusiness = userBusiness;
            this._bus = bus;
            this._cache = cache;
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
                    return BadRequest(new ResponseModel<string> { IsSuccess = false,Message="User Register Failed",Data="Email Already Exists" });
                }
                else
                {
                    return Ok(new ResponseModel<UserEntity> { IsSuccess = true,Message="Register successfull", Data=result});
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false,Message="User Register Failed",Data=ex.Message });
            }
        }
        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginModel loginModel)
        {
            try
            {
                if (userBusiness.CheckEmail(loginModel.Email))
                {
                    var result = userBusiness.Login(loginModel);
                    if (result == "Login Failed")
                    {
                        return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "User Login Failed", Data = "Login Failed" });
                    }
                    else
                    {
                        return Ok(new ResponseModel<string> { IsSuccess = true, Message = "User login Successfull", Data = result });
                    }
                }
                else 
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Email doesn't exist",Data="try to add the email first" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Email doesn't exist",Data=ex.Message });
            }
        }
        [HttpGet("FetchById")]
        public ActionResult FetchById(int id)
        {
            try
            {
                var result = userBusiness.FetchById(id);
                if(result != null)
                {
                    return Ok(new ResponseModel<UserEntity> { IsSuccess=true,Message="select by Id successfull",Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "UserId did not exist" });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("UpdateById")]
        public ActionResult UpdateById(int id,UpdateUserModel model) 
        {
            try
            {
                bool updateUser = userBusiness.UpdateUserDetails(id, model);
                if (updateUser)
                {
                    return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "user is updated by Id", Data =true  });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "UserId did not exist" });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                ForgotPasswordModel result = userBusiness.ForgotPassword(email);
                if (result != null)
                {
                    Send send = new Send();
                    ForgotPasswordModel forgotPasswordModel = userBusiness.ForgotPassword(email);
                    send.SendEmail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                    Uri uri = new Uri("rabbitmq:://localhost/FunDooNotesEmailQueue");
                    var endPoint = await _bus.GetSendEndpoint(uri);
                    await endPoint.Send(forgotPasswordModel);
                    return Ok(new ResponseModel<ForgotPasswordModel> { IsSuccess = true, Message = "execution successfull", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "Email doesn't exist" });
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel Model)
        {
            string Email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            if (Email!=null)
            {
                if(userBusiness.ResetPassword(Email, Model))
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Updated Password", Data = "This is the Updated password : "+Model.ConfirmPassword+" for Email of"+Email });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "Password is not equal to confirm Password" });
                }
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Failed", Data = "Email doesn't exist." });
            }
        }
        [HttpGet("AllUserWithRedis")]
        public async Task<IActionResult> GetAllUserWithRedis()
        {
            string cacheKey = "UsersList";
            string serializationUserList;
            var UsersList = new List<UserEntity>();
            var RedisUsersList = await _cache.GetAsync(cacheKey);
            if (RedisUsersList != null)
            {
                serializationUserList=Encoding.UTF8.GetString(RedisUsersList);
                UsersList = JsonConvert.DeserializeObject<List<UserEntity>>(serializationUserList);
            }
            else
            {
                UsersList = userBusiness.GetAllUsers();
                serializationUserList=JsonConvert.SerializeObject(UsersList);
                RedisUsersList = Encoding.UTF8.GetBytes(serializationUserList);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(11))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await _cache.SetAsync(cacheKey, RedisUsersList,options);
            }
            return Ok(UsersList);
        }
        [HttpGet("UpdateOrInsert")]
        public ActionResult UpdateOrInsert(int userId,RegisterModel register)
        {
            try
            {
                string result = userBusiness.CheckCreate(userId,register);
                if (result == "Update the data" || result == "Insert the data")
                {
                    return Ok(new ResponseModel<string> { IsSuccess = true, Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccess=false, Data = result });
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}