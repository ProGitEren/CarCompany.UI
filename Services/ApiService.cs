using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;
using CarCompany.UI.DTO;
using CarCompany.UI.Models.ViewModels;
using System.Net.Http.Headers;
using CarCompany.UI.Exceptions;
using Infrastucture.Errors;
using System.Net;
using CarCompany.UI.Errors;
using AutoMapper;
using NuGet.Protocol;
using System.Collections.Generic;

namespace CarCompany.UI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterAsync(RegisterViewModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
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
            if (!(response.StatusCode == HttpStatusCode.BadRequest))
            {
                var errormessage = await response.Content.ReadAsStringAsync();
                var ErrorObject = JsonConvert.DeserializeObject<BaseCommonResponse>(errormessage);
                throw new UIException(response.StatusCode, ErrorObject.Message);
            }
                var errormessage_ = await response.Content.ReadAsStringAsync();
                var ErrorObject_ = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(errormessage_);
                throw new UIException(response.StatusCode, ErrorObject_.Errors.FirstOrDefault());

        }

        public async Task<UserDto> LoginAsync(LoginViewModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/User/login", content);

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

            var errormessage = await response.Content.ReadAsStringAsync();
            var ErrorObject = JsonConvert.DeserializeObject<BaseCommonResponse>(errormessage);

            throw new UIException(response.StatusCode, ErrorObject.Message);
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

            var errormessage = await response.Content.ReadAsStringAsync();
            var ErrorObject = JsonConvert.DeserializeObject<ApiException>(errormessage);

            throw new UIException(response.StatusCode, ErrorObject.Message);
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

            var errormessage = await response.Content.ReadAsStringAsync();
            var ErrorObject = JsonConvert.DeserializeObject<ApiException>(errormessage);

            throw new UIException(response.StatusCode, ErrorObject.Message);
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

            var errormessage = await response.Content.ReadAsStringAsync();
            var ErrorObject = JsonConvert.DeserializeObject<ApiException>(errormessage);

            throw new UIException(response.StatusCode, ErrorObject.Message);
        }


       

        // Method to add token to requests
        public void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["JwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }


    }
}
