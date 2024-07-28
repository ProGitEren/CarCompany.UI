using System.ComponentModel.DataAnnotations;

namespace CarCompany.UI.DTO
{
    public class UserwithaddressDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public AddressDto AddressDto { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
