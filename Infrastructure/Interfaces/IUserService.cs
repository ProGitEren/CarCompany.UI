using AutoMapper;
using Infrastructure.Errors;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DTO.Dto_Users;
using Infrastructure.Models.ViewModels.Users;
using Infrastructure.Models.ViewModels.Addresses;
using Infrastucture.Helpers;
using Infrastucture.Params;

namespace Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterDto dto);

        Task<UserDto> RegisterAdminAsync(RegisterDto dto);

        Task<UserDto> LoginAsync(LoginDto dto);

        Task<UserViewModel> ProfileAsync();

        Task<IReadOnlyList<UserViewModel>> UsersListAsync();

        Task<Pagination<UserwithdetailDto>> GetUsersAsync(UserParams userParams);


        Task<UserViewModel> ProfileByEmailAsync(string email);

        Task<AddressViewModel> UpdateAddressAsync(AddressViewModel model);


        Task<UserDto> UpdateUserAsync(UpdateUserViewModel model);

        Task<string> DeleteUserAsync(string? email);

        Task<AddressViewModel> GetAddressAsync(Guid? Id);

     
           

       
    }
}
