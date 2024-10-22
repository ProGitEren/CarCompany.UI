using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Dto_Users
{
    public class LoginDto
    {
        public string Email { get; set; }

        public string EncryptedPassword { get; set; }
    }
}
