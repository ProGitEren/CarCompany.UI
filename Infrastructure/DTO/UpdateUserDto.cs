using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class UpdateUserDto
    {

        public string FirstName { get; set; }

        public string Email { get; set; }


        public string LastName { get; set; }

        
    }
}
