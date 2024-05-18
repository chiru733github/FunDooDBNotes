using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using Model_Layer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepo userRepo;
        public UserBusiness(IUserRepo repo)
        {
            this.userRepo = repo;
        }
        public UserEntity Register(RegisterModel register)
        {
            return userRepo.Register(register);
        }
        public string Login(LoginModel login)
        {
            return userRepo.Login(login);
        }
        public bool CheckEmail(string email)
        {
            return userRepo.CheckEmail(email);
        }
        public UserEntity FetchById(int UserId)
        {
            return userRepo.FetchById(UserId);
        }
        public bool UpdateUserDetails(int userid, UpdateUserModel user)
        {
            return userRepo.UpdateUserDetails(userid, user);
        }
        public ForgotPasswordModel ForgotPassword(string email)
        {
            return userRepo.ForgotPassword(email);
        }
        public bool ResetPassword(string Email,ResetPasswordModel reset)
        {
            return userRepo.ResetPassword(Email,reset);
        }
        public List<UserEntity> GetAllUsers()
        {
            return userRepo.GetAllUsers();
        }
        public string CheckCreate(int UserId,RegisterModel registerModel)
        {
            return userRepo.CheckCreate(UserId,registerModel);
        }
    }
}
