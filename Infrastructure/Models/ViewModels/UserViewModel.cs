using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }

        public string Email { get; set; }

        public AddressViewModel Address { get; set; }

        public VehicleViewModel Vehicle { get; set; }

        public string LastName { get; set; }

        public IList<string> roles { get; set; }

       

    }
}
