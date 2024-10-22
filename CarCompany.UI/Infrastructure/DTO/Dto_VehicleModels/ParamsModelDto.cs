using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.Dto_VehicleModels
{
    public class ModelParams
    {
        public int TotalItems { get; set; }

        public int PageItemCount { get; set; }
        public List<VehicleModelDto> VehicleModelDtos { get; set; }
    }
}
