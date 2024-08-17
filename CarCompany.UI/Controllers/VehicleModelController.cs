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
using Infrastucture.Helpers;
using Infrastucture.Params;

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
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "GetModels");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> PaginatedModels(int? StartYear,int? EndYear, int? filterModelId,
            string? sortoptions , string? searchInput ,string? vehicleType ,int PageNumber = 1, int PageSize = 10)
        {
            
            var param = new VehiclemodelParams
            {
                Search = searchInput,
                Sorting = sortoptions,
                StartingModelYear = StartYear,
                EndingModelYear = EndYear,
                ModelId = filterModelId,
                PageNumber = PageNumber,
                Pagesize = PageSize,
                VehicleType = vehicleType
            };

            // ViewDatas
            ViewData["StartYear"] = StartYear;
            ViewData["EndYear"] = EndYear;
            ViewData["filterModelId"] = filterModelId;
            ViewData["sortoptions"] = sortoptions;
            ViewData["searchInput"] = searchInput;
            ViewData["vehicleType"] = vehicleType;

            try
            {
                var result = await _vehicleModelService.GetModelsAsync(param) ;
                if (result == null)
                {
                    ModelState.AddModelError("", "The Models could not be found.");
                    _logger.Warning("Model retrieval failed: Model could not found");
                }
                _logger.Information("Model retrieval successful.");
                var model = _mapper.Map<Pagination<VehicleModelUserViewModel>>(result);
                return View(model);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "GetModels");
            }

            return RedirectToAction("ModelList","VehicleModel");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {


            var model = new RegisterVehicleModelViewModel();


            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create(RegisterVehicleModelViewModel model)
        {

            try
            {
                var vehicle = await _vehicleModelService.CreateAsync(model);
                _logger.Information("The Model successfully created.");
                return RedirectToAction("ModelList", "VehicleModel");
            }

            catch (Exception ex)
            {
                TempData["error"] = "Problem occured on registering the new model.";
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "CreateModel");

            }

            return View(model);
        }




        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int? Id)
        {
            var model = new UpdateVehicleModelViewModel();
            try
            {
                model = _mapper.Map<UpdateVehicleModelViewModel>(await _vehicleModelService.GetModelAsync(Id));
                if (model == null)
                {
                    ModelState.AddModelError("", "The Model could not be found.");
                    _logger.Warning("Model retrieval failed.");
                }
                
                _logger.Information("Model retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateModel");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateVehicleModelViewModel model)
        {

            try
            {
                var resultmodel = await _vehicleModelService.UpdateVehicleModelAsync(model);
                if (resultmodel == null)
                {
                    ModelState.AddModelError("", "The Model could not be found.");
                    _logger.Warning("Model update failed.");
                }
                _logger.Information("Model update successful.");
                return RedirectToAction("ModelList", "VehicleModel");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateModel");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? Id)
        {
            var model = new VehicleModelViewModel();
            try
            {
                model = await _vehicleModelService.GetModelAsync(Id);
                if (model == null)
                {
                    ModelState.AddModelError("", "The Model could not be found.");
                    _logger.Warning("Model retrieval failed.");
                }
                _logger.Information("Model retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "DeleteModel");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int? Id)
        {

            try
            {
                var stringresult = await _vehicleModelService.DeleteVehicleModelAsync(Id);
                if (stringresult == null)
                {
                    ModelState.AddModelError("", "The Model could not be found.");
                    _logger.Warning("Model delete failed.");
                }
                TempData["success"] = "The model successfully deleted.";
                _logger.Information("Model delete successful.");

            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "DeleteModel");
            }

            return RedirectToAction("ModelList", "VehicleModel");
        }





    }
}