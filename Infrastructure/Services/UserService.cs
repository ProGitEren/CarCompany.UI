using Newtonsoft.Json;
using System.Text;
using System.Security.Claims; 
using Infrastructure.DTO;
using Infrastructure.Models.ViewModels;
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

                // Store the token in a cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30),


                };

                _httpContextAccessor.HttpContext.Response.Cookies.Append("JwtToken", user.Token, cookieOptions);

                return user;
            }
            await ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type
            
            throw new UIException(response.StatusCode, "Registration failed.");

        }

        public async Task<UserDto> RegisterAdminAsync(RegisterDto model)
        {
            this.AddAuthorizationHeader(); // Since authorized user does this action we need this
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

            await ErrorResult(response);

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

                // Store the token in a cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30),


                };

                _httpContextAccessor.HttpContext.Response.Cookies.Append("JwtToken", user.Token, cookieOptions);

                return user;
            }

            await ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Login failed.");
        }

        public async Task<UserViewModel> ProfileAsync()
        {

            this.AddAuthorizationHeader();

            var response = await _httpClient.GetAsync("api/User/get-current-user-with-Address");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserwithaddressDto>(json);

                var model = _mapper.Map<UserViewModel>(user);

                return model;
            }

            await ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }

        public async Task<IReadOnlyList<UserViewModel>> UsersListAsync()
        {

            this.AddAuthorizationHeader();

            var response = await _httpClient.GetAsync("api/User/get-all-users-with-Address");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<IReadOnlyList<UsernotokenDto>>(json);

                var model = _mapper.Map<IReadOnlyList<UserViewModel>>(users);

                return model;
            }

            await ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");


        }

        public async Task<UserViewModel> ProfileByEmailAsync(string email)
        {

            this.AddAuthorizationHeader();

            var response = await _httpClient.GetAsync($"api/User/get-user-by-email-with-Address/{email}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UsernotokenDto>(json);

                var model = _mapper.Map<UserViewModel>(user);

                return model;
            }

            await ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }

        public async Task<AddressViewModel> UpdateAddressAsync(AddressViewModel model)
        {

            this.AddAuthorizationHeader();

            var content = new StringContent(JsonConvert.SerializeObject(_mapper.Map<AddressDto>(model)), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/User/update-user-Address", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var address = JsonConvert.DeserializeObject<AddressDto>(json);

                model = _mapper.Map<AddressViewModel>(address);

                return model;
            }

            await ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }

        public async Task<UserDto> UpdateUserAsync(UpdateUserViewModel model)
        {

            this.AddAuthorizationHeader();

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/User/update-user", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var userdto = JsonConvert.DeserializeObject<UserDto>(json);



                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30),


                };

                _httpContextAccessor.HttpContext.Response.Cookies.Delete("JwtToken");
                _httpContextAccessor.HttpContext.Response.Cookies.Append("JwtToken", userdto.Token, cookieOptions);

                return userdto;
            }

            await ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }

        public async Task<string> DeleteUserAsync(string? email)
        {

            this.AddAuthorizationHeader();

            var response = await _httpClient.DeleteAsync($"api/User/delete-user/{email}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<string>(json);

                return result;
            }

            await ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }



        public async Task<AddressViewModel> GetAddressAsync(Guid? Id)
        {

            this.AddAuthorizationHeader();

            var response = await _httpClient.GetAsync($"api/User/get-Address-by-id/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<AddressDto>(json);

                var model = _mapper.Map<AddressViewModel>(user);

                return model;
            }

            await ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Registration failed.");
        }

        // Method to add token to requests
        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["JwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task ErrorResult(HttpResponseMessage? response)
        {


            if (!(response.StatusCode == HttpStatusCode.BadRequest))
            {
                var errormessage = await response.Content.ReadAsStringAsync();
                var ErrorObject = JsonConvert.DeserializeObject<BaseCommonResponse>(errormessage);
                throw new UIException(response.StatusCode, ErrorObject.Message);
                
            }
            var errormessage_ = await response.Content.ReadAsStringAsync();
            var ErrorObject_ = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(errormessage_);
           
            throw new UIBadRequestException(response.StatusCode, ErrorObject_.Errors);
        }
    }
}



