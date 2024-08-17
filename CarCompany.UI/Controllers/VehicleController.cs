using AutoMapper;
using Infrastructure.DTO.Dto_Vehicles;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models.ViewModels.Users;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastructure.Models.ViewModels.Vehicles;
using Infrastructure.Services;
using Infrastucture.Helpers;
using Infrastucture.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarCompany.UI.Controllers
{
    public class VehicleController : Controller
    {
        private readonly VehicleService _vehicleService;
        private readonly UserService _userService;
        //private readonly VehicleModelService _vehicleModelService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;


        public VehicleController(VehicleService vehicleService, IMapper mapper, Serilog.ILogger logger, UserService userservice/*, VehicleModelService vehiclemodelservice*/)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
            _logger = logger;
            _userService = userservice;
            //_vehicleModelService = vehiclemodelservice;

        }


        [HttpGet]
        [Authorize(Roles = "Seller,Buyer")]
        public async Task<IActionResult> UserVehicles()
        {
            var model = new List<VehicleViewModel>();
            try
            {
                model = await _vehicleService.GetUserVehiclesAsync() as List<VehicleViewModel>;
                if (model == null)
                {
                    ModelState.AddModelError("", "The Vehicles could not be found.");
                    _logger.Warning("User vehicles retrieval failed: User not found");
                }
                _logger.Information("User vehicles retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UserVehicles");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? Id)
        {


            var model = new RegisterVehicleViewModel { ModelId = Id };
            if (Id == null)
            {
                ExceptionHelper.HandleException(new UIException(System.Net.HttpStatusCode.NotFound, "The Id of the model is null"), null, _logger, ModelState, "CreateVehicle");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterVehicleViewModel model)
        {

            try
            {
                var vehicle = await _vehicleService.CreateAsync(model);
                return User.IsInRole("Admin") ? RedirectToAction("Vehicles", "Vehicle") : RedirectToAction("UserVehicles", "Vehicle");
            }

            catch (Exception ex)
            {
                TempData["error"] = "Problem occured on registering the new vehicle.";
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "CreateVehicle");

            }

            return View(model);


        }

        [HttpGet]
        public async Task<IActionResult> VehicleDetail(string? email)
        {
            var vehicle = new List<VehicleViewModel>();
            try
            {
                vehicle = await _vehicleService.GetVehicleDetailAsync(email) as List<VehicleViewModel>;
                _logger.Information($"The vehicles for the user {email} has successfully retrieved");
                return View(vehicle);
            }

            catch (Exception ex)
            {
                TempData["error"] = "Problem occured on Retrieving the user vehicles.";
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "VehicleDetail");

            }

            return View(vehicle);
        }

        [HttpGet]
        public async Task<IActionResult> Vehicles()
        {
            var model = new List<VehicleViewModel>();
            try
            {
                model = await _vehicleService.GetVehiclesAsync() as List<VehicleViewModel>;
                if (model == null)
                {
                    ModelState.AddModelError("", "The Vehicles could not be found.");
                    _logger.Warning("Vehicles retrieval failed.");
                }
                _logger.Information("Vehicles retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> PaginatedVehicles(string? role,string? filterUserId, int? filterModelId,int? filterEngineId, string? filterVehicleId,
           string? sortoptions, string? searchInput, int PageNumber = 1, int PageSize = 10)
        {

            var param = new VehicleParams
            {
                Search = searchInput,
                Sorting = sortoptions,
                ModelId = filterModelId,
                VehicleId = filterVehicleId,
                EngineId = filterEngineId,
                PageNumber = PageNumber,
                Pagesize = PageSize,
                Role = role,
                UserId = filterUserId,
            };

            // ViewDatas
            ViewData["role"] = role;
            ViewData["filterUserId"] = filterUserId;
            ViewData["filterEngineId"] = filterEngineId;
            ViewData["filterModelId"] = filterModelId;
            ViewData["sortoptions"] = sortoptions;
            ViewData["searchInput"] = searchInput;

            try
            {
                var result = await _vehicleService.GetVehiclesAsync(param);
                if (result == null)
                {
                    ModelState.AddModelError("", "The Models could not be found.");
                    _logger.Warning("Model retrieval failed: Model could not found");
                }
                _logger.Information("Model retrieval successful.");
                var model = _mapper.Map<Pagination<VehicleViewModel>>(result);
                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "GetModels");
            }

            return RedirectToAction("ModelList", "VehicleModel");
        }




        [HttpGet]
        public async Task<IActionResult> Update(string? Id)
        {
            var model = new VehicleViewModel();
            try
            {
                model = await _vehicleService.GetVehicleAsync(Id);
                if (model == null)
                {
                    ModelState.AddModelError("", "The Vehicle could not be found.");
                    _logger.Warning("Vehicle retrieval failed.");
                }
                _logger.Information("Vehicle retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(VehicleViewModel model)
        {

            try
            {
                model = await _vehicleService.UpdateVehicleAsync(model);
                if (model == null)
                {
                    ModelState.AddModelError("", "The Vehicle could not be found.");
                    _logger.Warning("Vehicle update failed.");
                }
                _logger.Information("Vehicle update successful.");
                return User.IsInRole("Admin") ? RedirectToAction("Vehicles", "Vehicle") : RedirectToAction("UserVehicles", "Vehicle");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? Id)
        {
            var model = new VehicleViewModel();
            try
            {
                model = await _vehicleService.GetVehicleAsync(Id);
                if (model == null)
                {
                    ModelState.AddModelError("", "The Vehicle could not be found.");
                    _logger.Warning("Vehicle retrieval failed.");
                }
                _logger.Information("Vehicle retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string? Vin)
        {

            try
            {
                var stringresult = await _vehicleService.DeleteVehicleAsync(Vin);
                if (stringresult == null)
                {
                    ModelState.AddModelError("", "The Vehicle could not be found.");
                    _logger.Warning("Vehicle delete failed.");
                }
                TempData["success"] = "The vehicle successfully deleted.";
                _logger.Information("Vehicle delete successful.");
                return User.IsInRole("Admin") ? RedirectToAction("Vehicles", "Vehicle") : RedirectToAction("UserVehicles", "Vehicle");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
            }

            return User.IsInRole("Admin") ? RedirectToAction("Vehicles", "Vehicle") : RedirectToAction("UserVehicles", "Vehicle");
        }




    }
}

