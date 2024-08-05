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
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [MaxLength(25, ErrorMessage = " Maximum 25 characters")]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(25, ErrorMessage = " Maximum 25 characters")]
            public string LastName { get; set; }

            [Required]
            public DateTime birthtime { get; set; }

            [Required]
            public string EncryptedPassword { get; set; }

          
            [Required]
            public RegisterAddressViewModel Address { get; set; }

            [Required]
            public string Role { get; set; }

        }
}



