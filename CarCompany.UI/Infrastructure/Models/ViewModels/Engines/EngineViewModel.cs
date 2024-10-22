using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Models.Enums.VehicleEnums;

namespace Infrastructure.Models.ViewModels.Engines
{
    public class EngineViewModel
    {
        public int? Id { get; set; }
        public Cylinder Cylinder { get; set; }
        public decimal Volume { get; set; }
        public int Hp { get; set; }
        public int CompressionRatio { get; set; }
        public int Torque { get; set; }
        public decimal diameterCm { get; set; }
        public string EngineCode { get; set; }
        public string EngineName { get; set; }

        public Dictionary<Cylinder, string> CylinderTypeOptions { get; set; } = Enum.GetValues(typeof(Cylinder)).Cast<Cylinder>().ToDictionary(v => v, v => v.ToString());

    }
}
