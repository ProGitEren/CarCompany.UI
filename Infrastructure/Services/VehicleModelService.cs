using AutoMapper;
using Infrastructure.DTO.Dto_Users;
using Infrastructure.DTO.Dto_VehicleModels;
using Infrastructure.DTO.Dto_Vehicles;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastructure.Models.ViewModels.Vehicles;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Helpers;
using Infrastucture.Params;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class VehicleModelService :  IVehicleModelService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public VehicleModelService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<VehicleModelUserViewModel>> GetModelsAsync()
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.GetAsync("api/VehicleModel/get-all-models");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var models = JsonConvert.DeserializeObject<IEnumerable<VehicleModelDto>>(json);
                IReadOnlyList<VehicleModelUserViewModel> vehicles = _mapper.Map<IReadOnlyList<VehicleModelUserViewModel>>(models as IReadOnlyList<VehicleModelDto>);

                return vehicles;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");

        }

        public async Task<Pagination<VehicleModelDto>> GetModelsAsync(VehiclemodelParams modelParams) 
            {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this

            var builder = new StringBuilder();
            builder.Append($"?PageNumber={modelParams.PageNumber}");
            builder.Append($"&Pagesize={modelParams.Pagesize}");

            var properties = typeof(VehiclemodelParams).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(modelParams);
                if (value != null)
                {
                    var name = property.Name;
                    var stringValue = value.ToString();

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        if (name == nameof(modelParams.PageNumber) || name == nameof(modelParams.Pagesize) || name == nameof(modelParams.maxpagesize))
                            continue; // Skip these as they are already appended at the start.

                        // Check if the property type is nullable (like int?) or a simple string
                        if (property.PropertyType == typeof(int?) && (int?)value != null)
                        {
                            builder.Append($"&{name}={value}");
                        }
                        else if (property.PropertyType == typeof(string) && !string.IsNullOrEmpty(stringValue))
                        {
                            builder.Append($"&{name}={value}");
                        }
                    }
                }
            }

            var queryString = builder.ToString();
            var response = await _httpClient.GetAsync($"api/VehicleModel/get-models{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Pagination<VehicleModelDto>>(content);
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }
        public async Task<VehicleModelViewModel> CreateAsync(RegisterVehicleModelViewModel viewModel)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            //var dto = _mapper.Map<RegisterVehicleModelDto>(viewModel);
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(viewModel.ModelShortName), nameof(viewModel.ModelShortName));
            content.Add(new StringContent(viewModel.ModelLongName), nameof(viewModel.ModelLongName));
            content.Add(new StringContent(viewModel.ModelYear.ToString()), nameof(viewModel.ModelYear));
            content.Add(new StringContent(viewModel.VehicleType.ToString()), nameof(viewModel.VehicleType));
            content.Add(new StringContent(viewModel.EngineCode), nameof(viewModel.EngineCode));
            content.Add(new StringContent(viewModel.Price.ToString()), nameof(viewModel.Price));
            content.Add(new StringContent(viewModel.ManufacturedCountry.ToString()), nameof(viewModel.ManufacturedCountry));
            content.Add(new StringContent(viewModel.Manufacturer), nameof(viewModel.Manufacturer));
            content.Add(new StringContent(viewModel.ManufacturedPlant), nameof(viewModel.ManufacturedPlant));
            content.Add(new StringContent(viewModel.CheckDigit), nameof(viewModel.CheckDigit));

            if (viewModel.ModelPicture != null)
            {
                var fileContent = new StreamContent(viewModel.ModelPicture.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(viewModel.ModelPicture.ContentType);
                content.Add(fileContent, nameof(viewModel.ModelPicture), viewModel.ModelPicture.FileName);
            }
            var response = await _httpClient.PostAsync("api/VehicleModel/create-model", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vehiclemodel = JsonConvert.DeserializeObject<VehicleModelDto>(json);
                var model = _mapper.Map<VehicleModelViewModel>(vehiclemodel);

                return model;
            }
                await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");

        }


        public async Task<VehicleModelViewModel> GetModelAsync(int? Id)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.GetAsync($"api/VehicleModel/get-model/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vehiclemodel = JsonConvert.DeserializeObject<VehicleModelDto>(json);
                var model = _mapper.Map<VehicleModelViewModel>(vehiclemodel);

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }

        public async Task<VehicleModelViewModel> UpdateVehicleModelAsync(UpdateVehicleModelViewModel model)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this


            var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.ModelShortName), nameof(model.ModelShortName));
            content.Add(new StringContent(model.ModelLongName), nameof(model.ModelLongName));
            content.Add(new StringContent(model.ModelYear.ToString()), nameof(model.ModelYear));
            content.Add(new StringContent(model.VehicleType.ToString()), nameof(model.VehicleType));
            content.Add(new StringContent(model.EngineCode), nameof(model.EngineCode));
            content.Add(new StringContent(model.Price.ToString()), nameof(model.Price));
            content.Add(new StringContent(model.ManufacturedCountry.ToString()), nameof(model.ManufacturedCountry));
            content.Add(new StringContent(model.Manufacturer), nameof(model.Manufacturer));
            content.Add(new StringContent(model.ManufacturedPlant), nameof(model.ManufacturedPlant));
            content.Add(new StringContent(model.CheckDigit), nameof(model.CheckDigit));
            content.Add(new StringContent(model.Id.ToString()), nameof(model.Id));

            if (model.ModelPicture != null)
            {
                var fileContent = new StreamContent(model.ModelPicture.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.ModelPicture.ContentType);
                content.Add(fileContent, nameof(model.ModelPicture), model.ModelPicture.FileName);
            }

            var response = await _httpClient.PutAsync("api/VehicleModel/update-model", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vehiclemodel = JsonConvert.DeserializeObject<VehicleModelDto>(json);
                var resultmodel = _mapper.Map<VehicleModelViewModel>(vehiclemodel);

                return resultmodel;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }

        public async Task<string> DeleteVehicleModelAsync(int? Id)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.DeleteAsync($"api/VehicleModel/delete-model/{Id}");

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
