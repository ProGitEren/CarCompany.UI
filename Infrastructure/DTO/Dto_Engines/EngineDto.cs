
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.Dto_Engines
{
    public class EngineDto
    {

        public int? Id { get; set; }
        public enum Cylinder
        {
            [EnumMember(Value = "I2")]
            i2,
            [EnumMember(Value = "I4")]
            i4,
            [EnumMember(Value = "I6")]
            i6,
            [EnumMember(Value = "V4")]
            v4,
            [EnumMember(Value = "V6")]
            v6,
            [EnumMember(Value = "V8")]
            v8,
            [EnumMember(Value = "V10")]
            v10,
            [EnumMember(Value = "V12")]
            v12

        }
        public decimal Volume { get; set; }
        public int Hp { get; set; }
        public int CompressionRatio { get; set; }
        public int Torque { get; set; }
        public decimal diameterCm { get; set; }
        public string EngineCode { get; set; }

    }
}
