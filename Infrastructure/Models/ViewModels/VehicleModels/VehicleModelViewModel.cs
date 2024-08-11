using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Models.Enums.VehicleEnums;

namespace Infrastructure.Models.ViewModels.VehicleModels
{
    public class VehicleModelViewModel
    {

        public int? Id { get; set; }
        public VehicleType VehicleType { get; set; }

        public string EngineCode { get; set; }

        public string ModelShortName { get; set; }

        public string ModelLongName { get; set; }

        public int Quantity { get; set; }

        public int ModelYear { get; set; }

        public int ManufacturedCountry { get; set; } //1 number
        public string Manufacturer { get; set; } //2 letter
        public string securityCode { get; set; } // 1 letter
        public string ManufacturedYear { get; set; } // 1 letter
        public string ManufacturedPlant { get; set; }// 1 letter
        public string CheckDigit { get; set; }


    }
}
