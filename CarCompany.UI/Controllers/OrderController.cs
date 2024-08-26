using AutoMapper;
using Infrastructure.Helpers;
using Infrastructure.Models.ViewModels.Orders;
using Infrastructure.Models.ViewModels.OrderVehicles;
using Infrastructure.Params;
using Infrastructure.Services;
using Infrastucture.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarCompany.UI.Controllers
{
    public class OrderController : Controller
    {
        //private readonly VehicleService _vehicleService;
        private readonly OrderService _orderService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private readonly OrderVehicleService _orderVehicleService;


        public OrderController(IMapper mapper, Serilog.ILogger logger, OrderVehicleService orderVehicleService,OrderService orderService)
        {
            //_vehicleService = vehicleService;
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

                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "GetOrders");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Create(string? Id)
        {


            var model = new CreateOrderViewModel { OrderVehicle = new CreateOrderVehicleViewModel { VehicleId = Id } };

            return View(model);
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
                TempData["error"] = "Problem occured on registering the new order .";
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "CreateOrder");

            }

            return View(model);
        }




        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Update(int? Id)
        {
            var model = new UpdateOrderVehicleViewModel();
            try
            {
                model = _mapper.Map<UpdateOrderVehicleViewModel>(await _orderVehicleService.GetOrderVehicleAsync(Id));
                if (model == null)
                {
                    ModelState.AddModelError("", "The Order Vehicle could not be found.");
                    _logger.Warning("Order Vehicle retrieval failed.");
                }
                model.AddFile = new AddFileViewModel { Id = model.Id };
                _logger.Information("Order Vehicle retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateOrderVehicle");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateOrderVehicleViewModel model)
        {

            try
            {
                var resultmodel = await _orderVehicleService.UpdateOrderVehicleAsync(model);
                if (resultmodel == null)
                {
                    ModelState.AddModelError("", "The Order vehicle could not be found.");
                    _logger.Warning("Order Vehicle update failed.");
                }

                _logger.Information("Order vehicle update successful.");
                if (model.AddFile.Picture is not null)
                {
                    try
                    {
                        var stringresult = await _orderVehicleService.AddFileAsync(model.AddFile);

                        TempData["success"] = stringresult;

                    }
                    catch (Exception ex)
                    {
                        ExceptionHelper.HandleException(ex, null, _logger, ModelState, "AddFile");

                    }
                }
                // Split the comma-separated string into a list of paths
                if (!string.IsNullOrEmpty(model.ImagePathsToDelete))
                {
                    var pathsToDelete = model.ImagePathsToDelete.Split(',').ToList();
                    var deletefilemodel = new DeleteFileViewModel { Id = model.Id };
                    foreach (var imagePath in pathsToDelete)
                    {
                        deletefilemodel.FilePath = imagePath;
                        var result = await _orderVehicleService.DeleteFileAsync(deletefilemodel);

                    }
                }
                return RedirectToAction("PaginatedOrderVehicles", "OrderVehicle");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateOrderVehicle");
            }

            return View(model);

        }

        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Delete(int? Id)
        {
            var model = new OrderVehicleViewModel();
            try
            {
                model = await _orderVehicleService.GetOrderVehicleAsync(Id);
                if (model == null)
                {
                    ModelState.AddModelError("", "The Order Vehicle could not be found.");
                    _logger.Warning("Order Vehicle retrieval failed.");

                }
                _logger.Information("Order Vehicle retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "DeleteORderVehicle");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int? Id)
        {

            try
            {
                var stringresult = await _orderVehicleService.DeleteOrderVehicleAsync(Id);
                if (stringresult == null)
                {
                    ModelState.AddModelError("", "The Model could not be found.");
                    _logger.Warning("Order Vehicle delete failed.");
                }
                TempData["success"] = "The Order Vehicle successfully deleted.";
                _logger.Information("Order Vehicle delete successful.");

            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "DeleteORderVehicle");
            }

            return RedirectToAction("PaginatedOrderVehicles", "OrderVehicle");
        }


        
    }
}
