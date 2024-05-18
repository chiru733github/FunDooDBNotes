using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Model_Layer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FunDooDBContext funDooDBContext;
        private readonly IConfiguration configuration;
        public UserRepo(FunDooDBContext DBContext,IConfiguration Config)
        {
            this.funDooDBContext = DBContext;
            this.configuration = Config;
        }
        public UserEntity Register(RegisterModel register)
        {
            UserEntity? UserEmail = funDooDBContext.Users.ToList().Find(user => user.Email==register.Email);
            UserEntity userEntity = new UserEntity();
            if(UserEmail==null)
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
            return null;
        }
        public string Login(LoginModel loginModel)
        {
            var result = funDooDBContext.Users.FirstOrDefault(u => u.Email==loginModel.Email && u.Password==EncodePassword(loginModel.Password));
            if(result!=null)
            {
                var Token = GenerateToken(result.Email,result.UserId);
                return Token;
            }
            else
            {
                return "Login Failed";
            }
        }
        public ForgotPasswordModel ForgotPassword(string email)
        {
            if(CheckEmail(email))
            {
                ForgotPasswordModel forgotmodel = new ForgotPasswordModel();
                UserEntity User = funDooDBContext.Users.FirstOrDefault(user => user.Email == email);
                forgotmodel.Email = User.Email;
                forgotmodel.UserId = User.UserId;
                forgotmodel.Token = GenerateToken(User.Email,User.UserId);
                return forgotmodel;
            }
            return null;
        }
        public bool ResetPassword(string Email,ResetPasswordModel Reset) 
        {
            UserEntity User = funDooDBContext.Users.FirstOrDefault(user => user.Email == Email);
            if(Reset.Password==Reset.ConfirmPassword)
            {
                User.Password = EncodePassword(Reset.ConfirmPassword);
                User.ChangedAt = DateTime.Now;
                funDooDBContext.SaveChanges();
                return true;
            }
            return false;
        }
        public UserEntity FetchById(int UserId)
        {
            UserEntity User = funDooDBContext.Users.FirstOrDefault(user => user.UserId == UserId);
            return User;
        }
        public bool UpdateUserDetails(int userid, UpdateUserModel user)
        {
            var updateuser = funDooDBContext.Users.FirstOrDefault(x => x.UserId == userid);
            if (updateuser != null)
            {
                updateuser.FirstName = user.FirstName;
                updateuser.LastName = user.LastName;
                updateuser.Email = user.Email;
                funDooDBContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        //2) check for data existence of user using any table column, if user exist update the data else insert the data
        public string CheckCreate(int UserId,RegisterModel registerModel)
        {
            var result = (UserEntity)(from i in funDooDBContext.Users
                                            where i.UserId == UserId
                                            select i);
            if(result!=null)
            {
                result.FirstName = registerModel.FirstName;
                result.LastName = registerModel.LastName;
                result.Email = registerModel.Email;
                result.Password = EncodePassword(registerModel.Password);
                result.CreatedAt = DateTime.Now;
                funDooDBContext.SaveChanges();
                return "Update the data";
            }
            else
            {
                UserEntity newuser = new UserEntity();
                newuser.FirstName = registerModel.FirstName;
                newuser.LastName = registerModel.LastName;
                newuser.Email = registerModel.Email;
                newuser.Password = EncodePassword(registerModel.Password);
                newuser.CreatedAt = DateTime.Now;
                newuser.ChangedAt = DateTime.Now;
                funDooDBContext.Users.Add(newuser);
                funDooDBContext.SaveChanges();
                return "Insert the data.";
            }
           
        }
        public List<UserEntity> GetAllUsers()
        {
            List<UserEntity> result = funDooDBContext.Users.ToList();
            return result;
        }
        public bool CheckEmail(string Email)
        {
            UserEntity? UserEmail = funDooDBContext.Users.ToList().Find(user => user.Email == Email);
            return UserEmail!=null;
        }
        public string EncodePassword(string password)
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

        private string GenerateToken(string email,int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",email),
                new Claim("UserId",userId.ToString())
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}