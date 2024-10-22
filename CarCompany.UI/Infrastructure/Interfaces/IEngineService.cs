using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Mappings;
using Infrastructure.Models.ViewModels.Engines;
using Infrastucture.DTO.Dto_Engines;
using Infrastucture.Helpers;
using Infrastucture.Params;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IEngineService 
    {
        Task<IReadOnlyList<EngineViewModel>> GetEnginesAsync();


        Task<Pagination<EngineDto>> GetEnginesAsync(EngineParams engineParams);


        Task<EngineViewModel> CreateAsync(RegisterEngineViewModel model);

        Task<EngineViewModel> GetEngineAsync(int? Id);

        Task<EngineViewModel> UpdateEngineAsync(EngineViewModel model);

        Task<string> DeleteEngineAsync(int? Id);
        

    }
}
