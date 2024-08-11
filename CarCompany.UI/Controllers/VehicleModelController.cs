using AutoMapper;
using Infrastructure.Helpers;
using Infrastructure.Models.ViewModels.Vehicles;
using Infrastructure.Services;
using Infrastucture.DTO.Dto_VehicleModels;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models.ViewModels.VehicleModels;

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


       
    }
}
