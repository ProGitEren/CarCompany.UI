﻿using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTO.Dto_Users
{
    public class UserDto
    {

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public string Token { get; set; }
    }
}
