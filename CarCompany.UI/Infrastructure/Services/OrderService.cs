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
using Infrastucture.Params;
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
             // Since authorized user does this action we need this
            var dto = _mapper.Map<CreateOrderDto>(viewModel);
            var formDto = _mapper.Map<MultiFormCreateOrderDto>(dto);
            var content = new MultipartFormDataContent();
            
            if(formDto.BuyerEmail is not null)
                content.Add(new StringContent(formDto.BuyerEmail), nameof(formDto.BuyerEmail));
           
            content.Add(new StringContent(formDto.OrderVehicleDtoJson), nameof(formDto.OrderVehicleDtoJson));
            content.Add(new StringContent(formDto.TransferAddressJson), nameof(formDto.TransferAddressJson));

            content.Add(new StringContent(formDto.SellerEmail), nameof(formDto.SellerEmail));
            content.Add(new StringContent(formDto.OrderType.ToString()), nameof(formDto.OrderType));
            content.Add(new StringContent(formDto.PaymentMethod.ToString()), nameof(formDto.PaymentMethod));

                

            if (formDto.Images != null && formDto.Images.Any())
            {
                foreach (var image in formDto.Images)
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
            var response = await _httpClient.DeleteAsync($"api/Order/delete-order/{Id}");

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

        public async Task<OrderViewModel> GetOrderAsync(int? Id)
        {
            var response = await _httpClient.GetAsync($"api/Order/get-order/{Id}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return _mapper.Map<OrderViewModel>(JsonConvert.DeserializeObject<OrderDto>(content));
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");

        }

        public async Task<Pagination<OrderDto>> GetOrdersAsync(OrderParams orderParams)
        {
            // Since authorized user does this action we need this

            var queryString = PaginationBuilderHelper<OrderParams>.QueryString(orderParams);
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
            var dto = _mapper.Map<UpdateOrderDto>(model);
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/Order/update-order", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return _mapper.Map<OrderViewModel>(JsonConvert.DeserializeObject<OrderDto>(result));
            }

            await ErrorResultHelper.ErrorResult(response);

            // This line will never be reached, but is required by the compiler
            // because the method signature requires a UserDto return type

            throw new UIException(response.StatusCode, "Failed.");
        }
    }
}
