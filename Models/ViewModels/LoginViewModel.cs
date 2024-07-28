using System.ComponentModel.DataAnnotations;

namespace CarCompany.UI.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


    }
}
