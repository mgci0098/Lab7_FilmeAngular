using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.ViewModels
{
    public class RegisterPostModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        //[EmailAddress]    //am facut clasa de validari
        public string Email { get; set; }
        //[StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        public DateTime DataRegistered { get; set; }

    }
}
