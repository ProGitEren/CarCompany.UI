using AutoMapper;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models.ViewModels.Orders;
using Infrastructure.Models.ViewModels.OrderVehicles;
using Infrastructure.Params;
using Infrastructure.Services;
using Infrastucture.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using static Infrastructure.Models.Enums.OrderEnums;

namespace CarCompany.UI.Controllers
{
    public class OrderController : Controller
    {
        //private readonly VehicleService _vehicleService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private readonly IOrderVehicleService _orderVehicleService;


        public OrderController(IMapper mapper, Serilog.ILogger logger, IOrderVehicleService orderVehicleService,IOrderService orderService)
        {
            _mapper = mapper;
            _logger = logger;
            _orderService = orderService;
            _orderVehicleService = orderVehicleService;
            
        }

      

        [HttpGet]
        public async Task<IActionResult> PaginatedOrders(string? sortoptions, string? searchInput, string? filterVehicleId,string? orderType,string? orderStatus,string? paymentMethod,string? sellerEmail,string? buyerEmail ,int PageNumber = 1, int PageSize = 10)
        {

            var param = new OrderParams
            {
                Search = searchInput,
                Sorting = sortoptions,
                VehicleId = filterVehicleId,
                PageNumber = PageNumber,
                Pagesize = PageSize,
                OrderStatus = orderStatus,
                OrderType = orderType,
                BuyerEmail = buyerEmail,
                SellerEmail = sellerEmail,
                PaymentMethod = paymentMethod
            };

            // ViewDatas
            ViewData["filterVehicleId"] = filterVehicleId;
            ViewData["sortoptions"] = sortoptions;
            ViewData["searchInput"] = searchInput;
            ViewData["sellerEmail"] = sellerEmail;
            ViewData["buyerEmail"] = buyerEmail;
            ViewData["orderType"] = orderType;
            ViewData["orderStatus"] = orderStatus;
            ViewData["paymentMethod"] = paymentMethod;

           


            try
            {
                var result = await _orderService.GetOrdersAsync(param);
                if (result == null)
                {
                    ModelState.AddModelError("", "The Orders could not be found.");
                    _logger.Warning("Orders retrieval failed: Model could not found");
                }
                _logger.Information("Orders retrieval successful.");
                var model = _mapper.Map<Pagination<OrderViewModel>>(result);
                var ordervehicleparam = new OrderVehicleParams();

                ViewData["VehicleIdList"] = string.Join(",", (await _orderVehicleService.GetOrderVehiclesAsync(ordervehicleparam))
                    .Data.Select(x => x.VehicleId));

                IndexPageErrorsHelper.ShowTempDataErrors(ModelState, TempData);
                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "GetOrders");
            }

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("Index", "Home");
        }                                       

        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Create(string? Id)
        {
            var user = User;
            if (user == null) { ModelState.AddModelError("","Current user could not be found"); }
            var userEmail = user.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
        
            var model = new CreateOrderViewModel { SellerEmail = userEmail, OrderVehicle = new CreateOrderVehicleViewModel { VehicleId = Id } };
            if(ModelState.IsValid)
                return View(model);

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrders", "Order");
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]

        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {

            try
            {
                var order = await _orderService.CreateAsync(model);
                _logger.Information("The Order successfully created.");
                return RedirectToAction("PaginatedOrders", "Order");
            }

            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "CreateOrder");
            }

            return View(model);
        }




        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Update(int? Id)
        {
            var model = new UpdateOrderViewModel();
            try
            {
                model = _mapper.Map<UpdateOrderViewModel>(await _orderService.GetOrderAsync(Id));
                model.OrderVehicle = _mapper.Map <UpdateOrderVehicleViewModel>(await _orderVehicleService.GetOrderVehicleAsync(model.OrderVehicleId));
                if (model == null)
                {
                    ModelState.AddModelError("", "The Order could not be found.");
                    _logger.Warning("Order retrieval failed.");
                }
                model.OrderVehicle.AddFile = new AddFileViewModel { Id = model.OrderVehicleId };
                _logger.Information("Order retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateOrder");
            }
            if (ModelState.IsValid) 
                return View(model);

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrders", "Order");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateOrderViewModel model)
        {

            try
            {
                var resultmodel = await _orderService.UpdateOrderAsync(model);
                if (resultmodel == null)
                {
                    ModelState.AddModelError("", "The Order could not be found.");
                    _logger.Warning("Order update failed.");
                }

                _logger.Information("Order update successful.");
                if (model.OrderVehicle.AddFile.Picture is not null)
                {
                    try
                    {
                        var stringresult = await _orderVehicleService.AddFileAsync(model.OrderVehicle.AddFile);

                        TempData["success"] = stringresult;

                    }
                    catch (Exception ex)
                    {
                        ExceptionHelper.HandleException(ex, null, _logger, ModelState, "AddFile");

                    }
                }
                // Split the comma-separated string into a list of paths
                if (!string.IsNullOrEmpty(model.OrderVehicle.ImagePathsToDelete))
                {
                    var pathsToDelete = model.OrderVehicle.ImagePathsToDelete.Split(',').ToList();
                    var deletefilemodel = new DeleteFileViewModel { Id = model.OrderVehicleId };
                    foreach (var imagePath in pathsToDelete)
                    {
                        deletefilemodel.FilePath = imagePath;
                        var result = await _orderVehicleService.DeleteFileAsync(deletefilemodel);

                    }
                }
                if(ModelState.IsValid)
                    return RedirectToAction("PaginatedOrders", "Order");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateOrder");
            }
            
            return View(model);

        }

        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Delete(int? Id)
        {
            var model = new UpdateOrderViewModel();
            try
            {
                model = _mapper.Map<UpdateOrderViewModel>(await _orderService.GetOrderAsync(Id));
                model.OrderVehicle = _mapper.Map<UpdateOrderVehicleViewModel>(await _orderVehicleService.GetOrderVehicleAsync(model.OrderVehicleId));
                if (model == null)
                {
                    ModelState.AddModelError("", "The Order could not be found.");
                    _logger.Warning("Order retrieval failed.");

                }
                _logger.Information("Order retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "DeleteOrder");
            }
            if (ModelState.IsValid)
                return View(model);
            
            IndexPageErrorsHelper.ShowTempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrders", "Order");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int? Id)
        {

            try
            {
                var stringresult = await _orderService.DeleteOrderAsync(Id);
                if (stringresult == null)
                {
                    ModelState.AddModelError("", "The Order could not be found.");
                    _logger.Warning("Order delete failed.");
                }
                TempData["success"] = "The Order successfully deleted.";
                _logger.Information("Order delete successful.");

            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "DeleteOrder");
            }

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrders", "Order");
        }

        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> StatusSold(int? Id, string? buyerEmail)
        {
            if (Id is not null)
            {
                var order = await _orderService.GetOrderAsync(Id);

                if (order is not null)
                {
                    try
                    {
                        order.OrderStatus = OrderStatus.Sold;
                        order.BuyerEmail = buyerEmail;
                        var updatemodel = _mapper.Map<UpdateOrderViewModel>(order);
                        var ordervehicle = await _orderVehicleService.GetOrderVehicleAsync(order.OrderVehicleId);
                        if (ordervehicle is null) { return RedirectToAction("PaginatedOrders", "Order"); }
                        updatemodel.OrderVehicle = _mapper.Map<UpdateOrderVehicleViewModel>(ordervehicle);

                        var model = await _orderService.UpdateOrderAsync(updatemodel);
                        TempData["success"] = "Order Status successfully changed";
                    }
                    catch (Exception ex)
                    {
                        ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
                    }
                   
                }
                else 
                {
                     ModelState.AddModelError("", "The Order with this id could not found");
                }
            }
            else 
            {
                ModelState.AddModelError("", "The Id could not found");
            }


            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrders", "Order");
            
        }

        public async Task<IActionResult> Details(int? Id) 
        {
            
            if (Id is not null)
            {
                var model = await _orderService.GetOrderAsync(Id);
                if(model is not null) 
                {
                    return View(model);
                } 
                ModelState.AddModelError("", "The order with this Id could not found.");
            }
            ModelState.AddModelError("", "The received Id is null.");

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrders", "Order");
        
        }

    }
}
