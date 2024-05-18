using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model_Layer.Models;
using RepositoryLayer.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IUserBusiness
    {
        UserEntity Register(RegisterModel register);
        string Login(LoginModel login);
        bool CheckEmail(string email);
        UserEntity FetchById(int UserId);
        bool UpdateUserDetails(int userid, UpdateUserModel user);
        ForgotPasswordModel ForgotPassword(string email);
        bool ResetPassword(string Email, ResetPasswordModel reset);
        List<UserEntity> GetAllUsers();
        string CheckCreate(int UserId,RegisterModel registerModel);
    }
}