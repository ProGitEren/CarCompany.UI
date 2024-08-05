using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Errors;
using Infrastructure.Exceptions;
using Infrastructure.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterDto dto);

        Task<UserDto> RegisterAdminAsync(RegisterDto dto);

        Task<UserDto> LoginAsync(LoginDto dto);

        Task<UserViewModel> ProfileAsync();

        Task<IReadOnlyList<UserViewModel>> UsersListAsync();


        Task<UserViewModel> ProfileByEmailAsync(string email);

        Task<AddressViewModel> UpdateAddressAsync(AddressViewModel model);


        Task<UserDto> UpdateUserAsync(UpdateUserViewModel model);

        Task<string> DeleteUserAsync(string? email);

        Task<AddressViewModel> GetAddressAsync(Guid? Id);

     
           

       
    }
}
