using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.ViewModels
{
    public class UserWithRolesViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public AddressViewModel Address { get; set; }

        [Required]

        public string LastName { get; set; }

        [Required]

        public IList<string> roles { get; set; }

    }
}
