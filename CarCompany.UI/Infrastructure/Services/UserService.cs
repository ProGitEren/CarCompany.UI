﻿using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;
using System.Net.Http.Headers;
using Infrastructure.Exceptions;
using Infrastructure.Errors;
using System.Net;
using AutoMapper;
using NuGet.Protocol;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Infrastructure.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Build.Framework;
using Infrastructure.DTO.Dto_Users;
using Infrastructure.DTO.Dto_Addresses;
using Infrastructure.Models.ViewModels.Users;
using Infrastructure.Models.ViewModels.Addresses;
using Infrastructure.Helpers;
using Infrastucture.DTO.Dto_Engines;
using Infrastucture.Helpers;
using Infrastucture.Params;
using NuGet.Common;
using Infrastructure.Models;
using Infrastructure.Params;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IMapper mapper) 
        {
            _httpClient = httpClient;         
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto dto)
        {

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/User/Register", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDto>(json);
                var cookieValue = JsonConvert.SerializeObject(new CookieModel { Token = user.Token, Expiration = DateTime.UtcNow.AddMinutes(30) });


                // Store the token in a cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30),


                };

                _httpContextAccessor.HttpContext.Response.Cookies.Append("JwtToken", cookieValue, cookieOptions);

                return user;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type
            
            throw new UIException(response.StatusCode, "Registration failed.");

        }

        public async Task<UserDto> RegisterAdminAsync(RegisterDto model)
        {
            
            // Since authorized user does this action we need this
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/User/register-admin", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDto>(json);

                //// Store the token in a cookie
                //var cookieOptions = new CookieOptions
                //{
                //    HttpOnly = true,
                //    Secure = true,
                //    SameSite = SameSiteMode.Strict,
                //    Expires = DateTime.UtcNow.AddMinutes(30),


                //};

                //_httpContextAccessor.HttpContext.Response.Cookies.Append("JwtToken", user.Token, cookieOptions);

                return user;
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }

        public async Task<UserDto> LoginAsync(LoginDto model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/User/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDto>(json);
                var cookieValue = JsonConvert.SerializeObject(new CookieModel { Token = user.Token , Expiration = DateTime.UtcNow.AddMinutes(30)});

                // Store the token in a cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30),


                };

                _httpContextAccessor.HttpContext.Response.Cookies.Append("JwtToken", cookieValue, cookieOptions);

                return user;
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Login failed.");
        }

        public async Task<Pagination<UserwithdetailDto>> GetUsersAsync(UserParams userParams)
        {
            // Since authorized user does this action we need this

            var queryString = PaginationBuilderHelper<UserParams>.QueryString(userParams);
            var response = await _httpClient.GetAsync($"api/User/get-users{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Pagination<UserwithdetailDto>>(content);
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }

        public async Task<UserViewModel> ProfileAsync()
        {

            var response = await _httpClient.GetAsync("api/User/get-current-user-with-Detail");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserwithdetailDto>(json);

                var model = _mapper.Map<UserViewModel>(user);

                return model;
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }

        public async Task<IReadOnlyList<UserViewModel>> UsersListAsync()
        {

            


            var response = await _httpClient.GetAsync("api/User/get-all-users-with-Detail");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<IReadOnlyList<UsernotokenDto>>(json);

                var model = _mapper.Map<IReadOnlyList<UserViewModel>>(users);

                return model;
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");


        }

        public async Task<UserViewModel> ProfileByEmailAsync(string email)
        {

            var response = await _httpClient.GetAsync($"api/User/get-user-by-email-with-Detail/{email}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UsernotokenDto>(json);

                var model = _mapper.Map<UserViewModel>(user);

                return model;
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }

        public async Task<AddressViewModel> UpdateAddressAsync(AddressViewModel model)
        {

            var content = new StringContent(JsonConvert.SerializeObject(_mapper.Map<AddressDto>(model)), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/User/update-user-Address", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var address = JsonConvert.DeserializeObject<AddressDto>(json);

                model = _mapper.Map<AddressViewModel>(address);

                return model;
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }

        public async Task<UserDto> UpdateUserAsync(UpdateUserViewModel model)
        {

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/User/update-user", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var userdto = JsonConvert.DeserializeObject<UserDto>(json);
                var cookieValue = JsonConvert.SerializeObject(new CookieModel { Token = userdto.Token, Expiration = DateTime.UtcNow.AddMinutes(30) });


                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30),


                };

                _httpContextAccessor.HttpContext.Response.Cookies.Delete("JwtToken");
                _httpContextAccessor.HttpContext.Response.Cookies.Append("JwtToken", cookieValue , cookieOptions);

                return userdto;
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }

        public async Task<string> DeleteUserAsync(string? email)
        {

            var response = await _httpClient.DeleteAsync($"api/User/delete-user/{email}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                //var result = JsonConvert.DeserializeObject<string>(json);
                //no need for deserialzie as it is already a string gives an error
                return json;
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }



        public async Task<AddressViewModel> GetAddressAsync(Guid? Id)
        {

            var response = await _httpClient.GetAsync($"api/User/get-Address-by-id/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<AddressDto>(json);

                var model = _mapper.Map<AddressViewModel>(user);

                return model;
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }


       
    }
}



