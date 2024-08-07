using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.ViewModels
{
    public class UpdateUserViewModel
    {
        
        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        //[Required]

        //public IList<string> roles { get; set; }

    }
}
