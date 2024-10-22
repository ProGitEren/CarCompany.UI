
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Models.Enums.VehicleEnums;

namespace Infrastucture.DTO.Dto_Engines
{
    public class EngineDto
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

    }
}
