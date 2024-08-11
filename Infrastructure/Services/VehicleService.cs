using AutoMapper;
using Infrastructure.DTO.Dto_Users;
using Infrastructure.DTO.Dto_Vehicles;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models.ViewModels.Vehicles;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public VehicleService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<VehicleViewModel>> GetUserVehiclesAsync() 
                        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.GetAsync("api/User/get-current-user-with-Detail");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserwithdetailDto>(json);
                IReadOnlyList<VehicleViewModel> vehicles = _mapper.Map<IReadOnlyList<VehicleViewModel>>(user.VehiclesDto as IReadOnlyList<VehicleDto>);

                return vehicles;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");

        }

        public async Task<VehicleViewModel> CreateAsync(RegisterVehicleViewModel viewmodel)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var content = new StringContent(JsonConvert.SerializeObject(_mapper.Map<RegisterVehicleDto>(viewmodel)), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Vehicle/create-vehicle", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vehicle = JsonConvert.DeserializeObject<VehicleDto>(json);
                var model = _mapper.Map<VehicleViewModel>(vehicle);

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");

        }

        public async Task<IReadOnlyList<VehicleViewModel>> GetVehicleDetailAsync(string? email)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.GetAsync($"api/Vehicle/vehicle-detail/{email}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserwithdetailDto>(json);
                IReadOnlyList<VehicleViewModel> vehicles = _mapper.Map<IReadOnlyList<VehicleViewModel>>(user.VehiclesDto as IReadOnlyList<VehicleDto>);

                return vehicles;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");

        }

        public async Task<IReadOnlyList<VehicleViewModel>> GetVehiclesAsync() 
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.GetAsync($"api/Vehicle/get-vehicles");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vehicles = JsonConvert.DeserializeObject<IReadOnlyList<VehicleDto>>(json);
                IReadOnlyList<VehicleViewModel> model = _mapper.Map<IReadOnlyList<VehicleViewModel>>(vehicles);

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }

        public async Task<VehicleViewModel> GetVehicleAsync(string? Id)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.GetAsync($"api/Vehicle/get-vehicle/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vehicle = JsonConvert.DeserializeObject<VehicleDto>(json);
                var model = _mapper.Map<VehicleViewModel>(vehicle);

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }

        public async Task<VehicleViewModel> UpdateVehicleAsync(VehicleViewModel model)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this

            var content = new StringContent(JsonConvert.SerializeObject(_mapper.Map<VehicleDto>(model)), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/Vehicle/update-vehicle",content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vehicle = JsonConvert.DeserializeObject<VehicleDto>(json);
                model = _mapper.Map<VehicleViewModel>(vehicle);

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }

        public async Task<string> DeleteVehicleAsync(string? Id)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.DeleteAsync($"api/Vehicle/delete-vehicle/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                //string is returned
                return json;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }


    }
}
