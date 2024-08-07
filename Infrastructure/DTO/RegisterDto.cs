using Infrastructure.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    
        public class RegisterDto
        {
            
            public string Email { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public DateTime birthtime { get; set; }

            public string Phone { get; set; }

            public string EncryptedPassword { get; set; }

            public RegisterAddressViewModel Address { get; set; }

            public string Role { get; set; }

        }
}



