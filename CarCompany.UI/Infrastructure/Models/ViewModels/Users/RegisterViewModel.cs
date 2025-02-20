﻿using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.ViewModels.Addresses;

namespace Infrastructure.Models.ViewModels.Users
{
    public class RegisterViewModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime birthtime { get; set; }


        public string Password { get; set; }


        public string Phone { get; set; }

        [Compare("Password", ErrorMessage = "Passwords Do Not Match")]
        public string ConfirmPassword { get; set; }

        public RegisterAddressViewModel Address { get; set; }

        public string Role { get; set; }

    }
}
