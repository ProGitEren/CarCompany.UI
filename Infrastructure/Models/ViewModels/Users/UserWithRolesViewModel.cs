using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models.ViewModels.Addresses;
using Infrastructure.Models.ViewModels.Vehicles;

namespace Infrastructure.Models.ViewModels.Users
{
    public class UserWithRolesViewModel
    {
        public string FirstName { get; set; }

        public string Email { get; set; }

        public AddressViewModel Address { get; set; }

        public ICollection<VehicleViewModel> Vehicles { get; set; }

        public string LastName { get; set; }

        public IList<string> roles { get; set; }

    }
}
