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
        [Required]

        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]

        public string LastName { get; set; }

        
    }
}
