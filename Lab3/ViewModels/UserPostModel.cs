using Lab3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.ViewModels
{
    public class UserPostModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }

        public static User ToUser(UserPostModel userModel)
        {
            UserRole rol = Lab3.Models.UserRole.Regular;

            if (userModel.UserRole == "UserManager")
            {
                rol = Lab3.Models.UserRole.UserManager;
            }
            else if (userModel.UserRole == "Admin")
            {
                rol = Lab3.Models.UserRole.Admin;
            }

            return new User
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Username = userModel.UserName,
                Email = userModel.Email,
                Password = userModel.Password,
                UserRole = rol
            };
        }
    }
}
