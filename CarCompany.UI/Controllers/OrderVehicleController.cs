


using AutoMapper;
using Infrastructure.DTO.DTO_OrderVehicles;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models.ViewModels.OrderVehicles;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastructure.Params;
using Infrastructure.Services;
using Infrastucture.Helpers;
using Infrastucture.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarCompany.UI.Controllers
{
    public class OrderVehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IOrderVehicleService _orderVehicleService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;


        public OrderVehicleController(IVehicleService vehicleService,IMapper mapper, Serilog.ILogger logger, IOrderVehicleService ordervehicleservice)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
            _logger = logger;
            _orderVehicleService = ordervehicleservice;
        }

        [HttpGet]

        public async Task<IActionResult> Pictures(int? Id) 
        {
            var model = new OrderVehicleViewModel();
            try 
            {
               model = await _orderVehicleService.GetOrderVehicleAsync(Id);
            }
            catch (Exception ex)
            { 
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "GetOrderVehicle");

            }
            return View(model);

        }

        [HttpGet]
        [Authorize(Roles ="Admin,Seller")]
        public async Task<IActionResult> PaginatedOrderVehicles(string? sortoptions, string? searchInput, string? filterVehicleId, int PageNumber = 1, int PageSize = 10)
        {

            var param = new OrderVehicleParams
            {
                Search = searchInput,
                Sorting = sortoptions,
                VehicleId = filterVehicleId,
                PageNumber = PageNumber,
                Pagesize = PageSize,
            };

            // ViewDatas
            ViewData["filterVehicleId"] = filterVehicleId;
            ViewData["sortoptions"] = sortoptions;
            ViewData["searchInput"] = searchInput;

            try
            {
                var result = await _orderVehicleService.GetOrderVehiclesAsync(param);
                if (result == null)
                {
                    ModelState.AddModelError("", "The Order vehicles could not be found.");
                    _logger.Warning("Order Vehicle retrieval failed: Model could not found");
                }
                _logger.Information("Order Vehicle retrieval successful.");
                var model = _mapper.Map<Pagination<OrderVehicleViewModel>>(result);

                IndexPageErrorsHelper.ShowTempDataErrors(ModelState, TempData);
                return View(model);

            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "GetOrderVehicles");
            }
            
            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Create(string? Id)
        {


            var model = new CreateOrderVehicleViewModel { VehicleId = Id};
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]

        public async Task<IActionResult> Create(CreateOrderVehicleViewModel model)
        {

            try
            {
                var ordervehicle = await _orderVehicleService.CreateAsync(model);
                _logger.Information("The Order vehicle successfully created.");
                
                return  RedirectToAction("PaginatedOrderVehicles", "OrderVehicle");
            }

            catch (Exception ex)
            {
                TempData["error"] = "Problem occured on registering the new order vehicle.";
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "CreateOrderVehicle");

            }

            return View(model);
        }




        [HttpGet]
        [Authorize(Roles = "Admin,Seller")]
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
            if (ModelState.IsValid)
                return View(model);

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrderVehicles", "OrderVehicle");
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
                    if(ModelState.IsValid)
                        return RedirectToAction("PaginatedOrderVehicles", "OrderVehicle");
                }
                catch (Exception ex)
                {
                    ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateOrderVehicle");
                }

                return View(model);
            
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Sellen")]
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

            if (ModelState.IsValid)
                return View(model);

            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrderVehicles", "OrderVehicle");
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
            
            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrderVehicles", "OrderVehicle");


        }


        [HttpGet]

        public async Task<IActionResult> AddFile(AddFileViewModel model)
        {
            if (model.Id == null)
            {
                ModelState.AddModelError("", "The Id of the order Vehicle is not recevived");
                IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
                return RedirectToAction("PaginatedOrderVehicles", "OrderVehicle");
            }
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> AddFilePost(AddFileViewModel model) 
        {
            try 
            {
                var stringresult = await _orderVehicleService.AddFileAsync(model);

                TempData["success"] = stringresult;
                
            }
            catch (Exception ex) 
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "AddFile");

            }
            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrderVehicles", "OrderVehicle");
        }

        [HttpGet]

        public async Task<IActionResult> DeleteFile(DeleteFileViewModel model)
        {
            try
            {
                var stringresult = await _orderVehicleService.DeleteFileAsync(model);

                TempData["success"] = stringresult;

            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "DeleteFile");

            }
            IndexPageErrorsHelper.TempDataErrors(ModelState, TempData);
            return RedirectToAction("PaginatedOrderVehicles", "OrderVehicle");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int? Id)
        {
            var model = new OrderVehicleDetailsViewModel();
            try
            {
                var ordervehiclemodel = await _orderVehicleService.GetOrderVehicleAsync(Id);
                if (ordervehiclemodel == null)
                {
                    ModelState.AddModelError("", "The Order Vehicle could not be found.");
                    _logger.Warning("Order Vehicle retrieval failed.");

                }
                var vehiclemodel = await _vehicleService.GetVehicleAsync(ordervehiclemodel.VehicleId);
                if (vehiclemodel == null)
                {
                    ModelState.AddModelError("", "The Vehicle could not be found.");
                    _logger.Warning("Vehicle retrieval failed.");

                }
                model.OrderVehicle = ordervehiclemodel;
                model.Vehicle = vehiclemodel;
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "DetailsOrderVehicle");
            }

            return View(model);
        }


    }
}
