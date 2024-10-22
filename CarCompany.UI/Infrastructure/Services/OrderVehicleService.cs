using AutoMapper;
using Infrastructure.DTO.DTO_OrderVehicles;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models.ViewModels.OrderVehicles;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastructure.Params;
using Infrastucture.DTO.Dto_Engines;
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
    public class OrderVehicleService : IOrderVehicleService
    {
         
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public OrderVehicleService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IMapper mapper) 
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        

        public async Task<Pagination<OrderVehicleDto>> GetOrderVehiclesAsync(OrderVehicleParams orderVehicleParams)
            {

            var queryString = PaginationBuilderHelper<OrderVehicleParams>.QueryString(orderVehicleParams);
            var response = await _httpClient.GetAsync($"api/OrderVehicle/get-ordervehicles{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Pagination<OrderVehicleDto>>(content);
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }
        public async Task<OrderVehicleViewModel> CreateAsync(CreateOrderVehicleViewModel viewModel)
        {
            //var dto = _mapper.Map<RegisterVehicleModelDto>(viewModel);
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(viewModel.Price.ToString()), nameof(viewModel.Price));
            content.Add(new StringContent(viewModel.VehicleId), nameof(viewModel.VehicleId));
            content.Add(new StringContent(viewModel.ModelName), nameof(viewModel.ModelName));

            if (viewModel.Images != null && viewModel.Images.Any())
            {
                foreach (var image in viewModel.Images)
                {
                    var fileContent = new StreamContent(image.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                    content.Add(fileContent, nameof(viewModel.Images), image.FileName);
                }
            }
            var response = await _httpClient.PostAsync("api/OrderVehicle/create-ordervehicle", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vehiclemodel = JsonConvert.DeserializeObject<OrderVehicleDto>(json);
                var model = _mapper.Map<OrderVehicleViewModel>(vehiclemodel);

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
           

        }


        public async Task<OrderVehicleViewModel> GetOrderVehicleAsync(int? Id)
        {
            var response = await _httpClient.GetAsync($"api/OrderVehicle/get-ordervehicle/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var vehiclemodel = JsonConvert.DeserializeObject<OrderVehicleDto>(json);
                var model = _mapper.Map<OrderVehicleViewModel>(vehiclemodel);

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");

        }

        public async Task<OrderVehicleViewModel> UpdateOrderVehicleAsync(UpdateOrderVehicleViewModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(_mapper.Map<UpdateOrderVehicleDto>(model)),Encoding.UTF8,"application/json");

            var response = await _httpClient.PutAsync("api/OrderVehicle/update-ordervehicle", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var ordervehicle = JsonConvert.DeserializeObject<OrderVehicleDto>(json);
                var resultmodel = _mapper.Map<OrderVehicleViewModel>(ordervehicle);

                return resultmodel;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
            
        }

        public async Task<string> DeleteOrderVehicleAsync(int? Id)
        {
            var response = await _httpClient.DeleteAsync($"api/OrderVehicle/delete-ordervehicle/{Id}");

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

        public async Task<string> AddFileAsync(AddFileViewModel model)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.Id.ToString()), nameof(model.Id));
            if (model.Picture != null)
            {
                var fileContent = new StreamContent(model.Picture.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.Picture.ContentType);
                content.Add(fileContent, nameof(model.Picture), model.Picture.FileName);
            }

            var response = await _httpClient.PostAsync($"api/OrderVehicle/add-file/",content);
            
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

        public async Task<string> DeleteFileAsync(DeleteFileViewModel model)
        {
            var builder = new StringBuilder();
            builder.Append($"?Id={model.Id}");
            builder.Append($"&FilePath={model.FilePath}");

            var query = builder.ToString();
            
            var response = await _httpClient.DeleteAsync($"api/OrderVehicle/delete-file{query}");

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

