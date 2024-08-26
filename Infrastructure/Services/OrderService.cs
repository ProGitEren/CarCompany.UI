using AutoMapper;
using Infrastructure.DTO.DTO_Orders;
using Infrastructure.DTO.DTO_OrderVehicles;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models.ViewModels.Orders;
using Infrastructure.Models.ViewModels.OrderVehicles;
using Infrastructure.Params;
using Infrastucture.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public OrderService(IHttpContextAccessor httpContextAccessor, HttpClient htpClient, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = htpClient;
            _mapper = mapper;
        }

        public async Task<OrderViewModel> CreateAsync(CreateOrderViewModel viewModel)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this
            var dto = _mapper.Map<CreateOrderDto>(viewModel);
            var content = new MultipartFormDataContent();
            //content.Add(new StringContent(dto.OrderVehicleDto.Price.ToString()), nameof(dto.OrderVehicleDto.Price));
            //content.Add(new StringContent(dto.OrderVehicleDto.VehicleId), nameof(dto.OrderVehicleDto.VehicleId));
            //content.Add(new StringContent(dto.OrderVehicleDto.ModelName), nameof(dto.OrderVehicleDto.ModelName));
            
            if(viewModel.BuyerEmail is not null)
                content.Add(new StringContent(viewModel.BuyerEmail), nameof(viewModel.BuyerEmail));
            // Serialize the OrderVehicleDto to JSON and add it to the form data
            var orderVehicleJson = JsonConvert.SerializeObject(dto.OrderVehicleDto);
            content.Add(new StringContent(orderVehicleJson), nameof(dto.OrderVehicleDto));

            // Serialize the TransferAddress to JSON and add it to the form data
            var transferAddressJson = JsonConvert.SerializeObject(dto.TransferAddress);
            content.Add(new StringContent(transferAddressJson), nameof(dto.TransferAddress));

            content.Add(new StringContent(dto.SellerEmail), nameof(dto.SellerEmail));
            content.Add(new StringContent(dto.OrderType.ToString()), nameof(dto.OrderType));
            content.Add(new StringContent(dto.PaymentMethod.ToString()), nameof(dto.PaymentMethod));

            //content.Add(new StringContent(dto.TransferAddress.ZipCode.ToString()), nameof(dto.TransferAddress.ZipCode));
            //content.Add(new StringContent(dto.TransferAddress.Country), nameof(dto.TransferAddress.Country));
            //content.Add(new StringContent(dto.TransferAddress.City), nameof(dto.TransferAddress.City));
            //content.Add(new StringContent(dto.TransferAddress.Name), nameof(dto.TransferAddress.Name));

            if (dto.OrderVehicleDto.Images != null && dto.OrderVehicleDto.Images.Any())
            {
                foreach (var image in dto.OrderVehicleDto.Images)
                {
                    var fileContent = new StreamContent(image.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                    content.Add(fileContent, nameof(dto.OrderVehicleDto.Images), image.FileName);
                }
            }
            var response = await _httpClient.PostAsync("api/Order/create-order", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<OrderDto>(json);
                var model = _mapper.Map<OrderViewModel>(order);

                return model;
            }
            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");

        }

        public async Task<string> DeleteOrderAsync(int? Id)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderViewModel> GetOrderAsync(int? Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Pagination<OrderDto>> GetOrdersAsync(OrderParams orderParams)
        {
            AuthorizationHelper.AddAuthorizationHeader(_httpContextAccessor, _httpClient); // Since authorized user does this action we need this

            var builder = new StringBuilder();
            builder.Append($"?PageNumber={orderParams.PageNumber}");
            builder.Append($"&Pagesize={orderParams.Pagesize}");

            var properties = typeof(OrderParams).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(orderParams);
                if (value != null)
                {
                    var name = property.Name;
                    var stringValue = value.ToString();

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        if (name == nameof(orderParams.PageNumber) || name == nameof(orderParams.Pagesize) || name == nameof(orderParams.maxpagesize))
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
            var response = await _httpClient.GetAsync($"api/Order/get-orders{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Pagination<OrderDto>>(content);
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }
       

        public async Task<OrderViewModel> UpdateOrderAsync(UpdateOrderViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
