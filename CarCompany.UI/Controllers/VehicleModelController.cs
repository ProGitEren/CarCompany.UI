using AutoMapper;
using Infrastructure.Helpers;
using Infrastructure.Models.ViewModels.Vehicles;
using Infrastructure.Services;
using Infrastucture.DTO.Dto_VehicleModels;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models.ViewModels.VehicleModels;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using static Infrastructure.Models.Enums.VehicleEnums;
using Infrastructure.Mappings;

namespace CarCompany.UI.Controllers
{
    public class VehicleModelController : Controller
    {
        //private readonly VehicleService _vehicleService;
        private readonly VehicleModelService _vehicleModelService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;


        public VehicleModelController(/*VehicleService vehicleService,*/ IMapper mapper, Serilog.ILogger logger, VehicleModelService vehiclemodelservice)
        {
            //_vehicleService = vehicleService;
            _mapper = mapper;
            _logger = logger;
            _vehicleModelService = vehiclemodelservice;

        }

        [HttpGet]
        public async Task<IActionResult> ModelList()
        {
            var model = new List<VehicleModelUserViewModel>();
            try
            {
                model = await _vehicleModelService.GetModelsAsync() as List<VehicleModelUserViewModel>;
                if (model == null)
                {
                    ModelState.AddModelError("", "The Models could not be found.");
                    _logger.Warning("Model retrieval failed: Model could not found");
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {


            var model = new RegisterVehicleModelViewModel
            {
                CheckDigitOptions = SecurityCodeMapper.SecurityCodes,
                VehicleTypeOptions = Enum.GetValues(typeof(VehicleType))
                                         .Cast<VehicleType>()
                                         .ToDictionary(v => v, v => v.ToString()),

                EngineCodeOptions = EngineMapper.EngineMapping,
                ManufacturedCountryOptions = ManufacturingCountryMapper.CountryMapping,
                ManufacturedPlantOptions = ManufacturedPlantMapper.PlantMapping,
                ManufacturerOptions = ManufacturerMapper.ManufacturerMapping,

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterVehicleModelViewModel model)
        {

            try
            {
                var vehicle = await _vehicleModelService.CreateAsync(model);
            }

            catch (Exception ex)
            {
                TempData["error"] = "Problem occured on registering the new vehicle.";
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "CreateVehicle");

            }

            return User.IsInRole("Admin") ? RedirectToAction("Vehicles", "Vehicle") : RedirectToAction("UserVehicles", "Vehicle");


            //}

            //[HttpGet]
            //public async Task<IActionResult> VehicleDetail(string? email)
            //{

            //    try
            //    {
            //        var vehicles = await _vehicleService.GetVehicleDetailAsync(email);
            //        _logger.Information($"The vehicles for the user {email} has successfully retrieved");
            //        return View(vehicles);
            //    }

            //    catch (Exception ex)
            //    {
            //        TempData["error"] = "Problem occured on Retrieving the user vehicles.";
            //        ExceptionHelper.HandleException(ex, null, _logger, ModelState, "VehicleDetail");

            //    }

            //    return RedirectToAction("UsersList", "Account");
            //}

            //[HttpGet]
            //public async Task<IActionResult> Vehicles()
            //{
            //    var model = new List<VehicleViewModel>();
            //    try
            //    {
            //        model = await _vehicleService.GetVehiclesAsync() as List<VehicleViewModel>;
            //        if (model == null)
            //        {
            //            ModelState.AddModelError("", "The Vehicles could not be found.");
            //            _logger.Warning("Vehicles retrieval failed.");
            //        }
            //        _logger.Information("Vehicles retrieval successful.");
            //    }
            //    catch (Exception ex)
            //    {
            //        ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
            //    }

            //    return View(model);
            //}

            //[HttpGet]
            //public async Task<IActionResult> Update(string? Id)
            //{
            //    var model = new VehicleViewModel();
            //    try
            //    {
            //        model = await _vehicleService.GetVehicleAsync(Id);
            //        if (model == null)
            //        {
            //            ModelState.AddModelError("", "The Vehicle could not be found.");
            //            _logger.Warning("Vehicle retrieval failed.");
            //        }
            //        _logger.Information("Vehicle retrieval successful.");
            //    }
            //    catch (Exception ex)
            //    {
            //        ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
            //    }

            //    return View(model);
            //}

            //[HttpPost]
            //public async Task<IActionResult> Update(VehicleViewModel model)
            //{

            //    try
            //    {
            //        model = await _vehicleService.UpdateVehicleAsync(model);
            //        if (model == null)
            //        {
            //            ModelState.AddModelError("", "The Vehicle could not be found.");
            //            _logger.Warning("Vehicle update failed.");
            //        }
            //        _logger.Information("Vehicle update successful.");
            //    }
            //    catch (Exception ex)
            //    {
            //        ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
            //    }

            //    return User.IsInRole("Admin") ? RedirectToAction("Vehicles", "Vehicle") : RedirectToAction("UserVehicles", "Vehicle");
            //}

            //[HttpGet]
            //public async Task<IActionResult> Delete(string? Id)
            //{
            //    var model = new VehicleViewModel();
            //    try
            //    {
            //        model = await _vehicleService.GetVehicleAsync(Id);
            //        if (model == null)
            //        {
            //            ModelState.AddModelError("", "The Vehicle could not be found.");
            //            _logger.Warning("Vehicle retrieval failed.");
            //        }
            //        _logger.Information("Vehicle retrieval successful.");
            //    }
            //    catch (Exception ex)
            //    {
            //        ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
            //    }

            //    return View(model);
            //}

            //[HttpPost]
            //public async Task<IActionResult> DeletePost(string? Vin)
            //{

            //    try
            //    {
            //        var stringresult = await _vehicleService.DeleteVehicleAsync(Vin);
            //        if (stringresult == null)
            //        {
            //            ModelState.AddModelError("", "The Vehicle could not be found.");
            //            _logger.Warning("Vehicle delete failed.");
            //        }
            //        TempData["success"] = "The vehicle successfully deleted.";
            //        _logger.Information("Vehicle delete successful.");
            //    }
            //    catch (Exception ex)
            //    {
            //        ExceptionHelper.HandleException(ex, null, _logger, ModelState, "Vehicles");
            //    }

            //    return User.IsInRole("Admin") ? RedirectToAction("Vehicles", "Vehicle") : RedirectToAction("UserVehicles", "Vehicle");
            //}




        }
    }
}
