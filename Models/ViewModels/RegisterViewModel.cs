using System.ComponentModel.DataAnnotations;

namespace CarCompany.UI.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = " Maximum 25 characters")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = " Maximum 25 characters")]
        public string LastName { get; set; }

        [Required]
        public DateTime birthtime { get; set; }

        [Required]

        [DataType(DataType.Password, ErrorMessage = "Password must be at least 8 characters, at least one digit, at least one lowercase, at least one upper case and at least one special character needed.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public AddressViewModel Address { get; set; }

    }
}
