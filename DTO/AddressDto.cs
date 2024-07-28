using System.ComponentModel.DataAnnotations;

namespace CarCompany.UI.DTO
{
    public class AddressDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "The length of the Address name should not exceed 100 characters.")]
        public string name { get; set; }

        [Required]

        public string city { get; set; }

        [Required]

        public string state { get; set; }

        [Required]

        public int zipcode { get; set; }

        [Required]

        public string country { get; set; }

    }
}
