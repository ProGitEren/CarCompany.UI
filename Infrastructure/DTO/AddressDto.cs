using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTO
{
    public class AddressDto
    {

        public Guid AddressId { get; set; }

        public string name { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public int zipcode { get; set; }

        public string country { get; set; }

    }
}
