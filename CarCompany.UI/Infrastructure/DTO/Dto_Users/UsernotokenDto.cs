﻿using System.ComponentModel.DataAnnotations;
using Infrastructure.DTO.Dto_Addresses;
using Infrastructure.DTO.Dto_Vehicles;

namespace Infrastructure.DTO.Dto_Users
{
    public class UsernotokenDto
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public AddressDto AddressDto { get; set; }
        public ICollection<VehicleDto> VehiclesDto { get; set; }
        public IList<string> roles { get; set; }

    }
}