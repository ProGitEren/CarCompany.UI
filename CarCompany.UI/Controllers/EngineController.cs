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
using Infrastructure.Map;
using Infrastructure.Models.ViewModels.Engines;
using Microsoft.VisualStudio.TextTemplating;

namespace CarCompany.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EngineController : Controller
    {
        private readonly EngineService _engineService;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;


        public EngineController(IMapper mapper, Serilog.ILogger logger, EngineService engineService)
        {
            _mapper = mapper;
            _logger = logger;
            _engineService = engineService;
        }

        [HttpGet]
        public async Task<IActionResult> EngineList()
        {
            var model = new List<EngineViewModel>();
            try
            {
                model = await _engineService.GetEnginesAsync() as List<EngineViewModel>;
                if (model == null)
                {
                    ModelState.AddModelError("", "The Engines could not be found.");
                    _logger.Warning("Engines retrieval failed: Engines could not found");
                }
                _logger.Information("Engines retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "GetEngines");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {


            var model = new RegisterEngineViewModel();


            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create(RegisterEngineViewModel model)
        {

            try
            {
                var vehicle = await _engineService.CreateAsync(model);
                _logger.Information("The Engine successfully deleted.");
                return RedirectToAction("EngineList", "Engine");
            }

            catch (Exception ex)
            {
                TempData["error"] = "Problem occured on registering the new engine.";
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "CreateEngine");

            }

            return View(model);
        }




        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int? Id)
        {
            var model = new EngineViewModel();
            try
            {
                model = await _engineService.GetEngineAsync(Id);
                if (model == null)
                {
                    ModelState.AddModelError("", "The Engine could not be found.");
                    _logger.Warning("Engine retrieval failed.");
                }
                _logger.Information("Engine retrieval successful.");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateEngine");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EngineViewModel model)
        {

            try
            {
                model = await _engineService.UpdateEngineAsync(model);
                if (model == null)
                {
                    ModelState.AddModelError("", "The Engine could not be found.");
                    _logger.Warning("Engine update failed.");
                }
                _logger.Information("Engine update successful.");
                return RedirectToAction("EngineList", "Engine");
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "UpdateEngine");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? Id)
        {
            var model = new EngineViewModel();
            try
            {
                model = await _engineService.GetEngineAsync(Id);
                if (model == null)
                {
                    ModelState.AddModelError("", "The Engine could not be found.");
                    _logger.Warning("Engine retrieval failed.");
                }
                _logger.Information("Engine retrieval successful.");
                EngineMapper.EngineMapping.Remove(model.EngineCode);

            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "DeleteEngine");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int? Id)
        {

            try
            {
                var stringresult = await _engineService.DeleteEngineAsync(Id);
                if (stringresult == null)
                {
                    ModelState.AddModelError("", "The Engine could not be found.");
                    _logger.Warning("Engine delete failed.");
                }

                TempData["success"] = "The Engine successfully deleted.";
                _logger.Information("Engine delete successful.");

            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, null, _logger, ModelState, "DeleteEngine");
            }

            return RedirectToAction("EngineList", "Engine");
        }





    }
}