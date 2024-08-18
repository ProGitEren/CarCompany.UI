using AutoMapper;
using Infrastructure.DTO.Dto_Users;
using Infrastructure.DTO.Dto_Vehicles;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models.ViewModels.Engines;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastructure.Models.ViewModels.Vehicles;
using Infrastucture.DTO.Dto_Engines;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Helpers;
using Infrastucture.Params;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TextTemplating;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EngineService : IEngineService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public EngineService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<EngineViewModel>> GetEnginesAsync()
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.GetAsync("api/Engine/get-all-engines");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var engines = JsonConvert.DeserializeObject<IEnumerable<EngineDto>>(json);
                IReadOnlyList<EngineViewModel> model = _mapper.Map<IReadOnlyList<EngineViewModel>>(engines as IReadOnlyList<EngineDto>);

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");

        }

        public async Task<Pagination<EngineDto>> GetEnginesAsync(EngineParams engineParams)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this

            var builder = new StringBuilder();
            builder.Append($"?PageNumber={engineParams.PageNumber}");
            builder.Append($"&Pagesize={engineParams.Pagesize}");

            var properties = typeof(EngineParams).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(engineParams);
                if (value != null)
                {
                    var name = property.Name;
                    var stringValue = value.ToString();

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        if (name == nameof(engineParams.PageNumber) || name == nameof(engineParams.Pagesize) || name == nameof(engineParams.maxpagesize))
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
            var response = await _httpClient.GetAsync($"api/Engine/get-engines{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Pagination<EngineDto>>(content);
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }

        public async Task<EngineViewModel> CreateAsync(RegisterEngineViewModel model)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient);
            var content = new StringContent(JsonConvert.SerializeObject(_mapper.Map<RegisterEngineDto>(model)), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Engine/create-engine", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var engine = JsonConvert.DeserializeObject<EngineDto>(json);
                var viewmodel = _mapper.Map<EngineViewModel>(engine);
                EngineMapper.EngineMapping.Add(engine.EngineCode, engine.EngineName);

                return viewmodel;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");

        }


        public async Task<EngineViewModel> GetEngineAsync(int? Id)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.GetAsync($"api/Engine/get-engine/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var engine = JsonConvert.DeserializeObject<EngineDto>(json);
                var model = _mapper.Map<EngineViewModel>(engine);

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }

        public async Task<EngineViewModel> UpdateEngineAsync(EngineViewModel model)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this

            var content = new StringContent(JsonConvert.SerializeObject(_mapper.Map<EngineDto>(model)), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/Engine/update-engine", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var engine = JsonConvert.DeserializeObject<EngineDto>(json);
                model = _mapper.Map<EngineViewModel>(engine);
                EngineMapper.EngineMapping[engine.EngineCode] = engine.EngineName;

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }

        public async Task<string> DeleteEngineAsync(int? Id)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var response = await _httpClient.DeleteAsync($"api/Engine/delete-engine/{Id}");

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
