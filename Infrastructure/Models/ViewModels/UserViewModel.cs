using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.ViewModels
{
    public class UserViewModel
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
