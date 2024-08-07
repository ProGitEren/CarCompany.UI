using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTO
{
    public class UsernotokenDto
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public AddressDto AddressDto { get; set; }

        public VehicleDto VehicleDto { get; set; }
        public IList<string> roles { get; set; }

    }
}
