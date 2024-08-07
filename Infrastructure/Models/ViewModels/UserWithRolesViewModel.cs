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
        public string FirstName { get; set; }

        public string Email { get; set; }

        public AddressViewModel Address { get; set; }

        public VehicleViewModel Vehicle { get; set; }

        public string LastName { get; set; }

        public IList<string> roles { get; set; }

    }
}
