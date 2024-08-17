using Infrastructure.Mappings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Models.Enums.VehicleEnums;

namespace Infrastructure.Models.ViewModels.VehicleModels
{
    public class RegisterVehicleModelViewModel
    {
        public VehicleType VehicleType { get; set; }
        public string EngineCode { get; set; }
        public string ModelShortName { get; set; }
        public string ModelLongName { get; set; }
        public int ModelYear { get; set; }
        public int ManufacturedCountry { get; set; } // Selected value
        public string Manufacturer { get; set; } // Selected value
        public string ManufacturedPlant { get; set; } // Selected value
        public string CheckDigit { get; set; } // Selected value
        public IFormFile ModelPicture { get; set; }
        public decimal Price { get; set; }



        // Select Options
        public Dictionary<int, string> ManufacturedCountryOptions { get; set; } = ManufacturingCountryMapper.CountryMapping;
        public Dictionary<string,string> ManufacturerOptions { get; set; } = ManufacturerMapper.ManufacturerMapping;
        public Dictionary<string,string> ManufacturedPlantOptions { get; set; } =  ManufacturedPlantMapper.PlantMapping;
        public List<string> CheckDigitOptions { get; set; } = SecurityCodeMapper.SecurityCodes;
        public Dictionary<VehicleType, string> VehicleTypeOptions { get; set; } =  Enum.GetValues(typeof(VehicleType)).Cast<VehicleType>().ToDictionary(v => v, v => v.ToString());
        public Dictionary<string, string> EngineCodeOptions { get; set; } = EngineMapper.EngineMapping;

    }
}
