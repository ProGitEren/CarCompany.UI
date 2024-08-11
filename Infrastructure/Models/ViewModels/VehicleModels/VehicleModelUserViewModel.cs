using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Models.Enums.VehicleEnums;

namespace Infrastructure.Models.ViewModels.VehicleModels
{
    public class VehicleModelUserViewModel
    {
        public int? Id { get; set; }
        public VehicleType VehicleType { get; set; }

        public string EngineCode { get; set; }

        public string ModelShortName { get; set; }

        public string ModelLongName { get; set; }

        public int Quantity { get; set; }

        public int ModelYear { get; set; }
       

    }
}
