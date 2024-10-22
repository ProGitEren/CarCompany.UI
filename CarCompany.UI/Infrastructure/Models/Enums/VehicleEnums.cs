using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Enums
{
    public class VehicleEnums
    {
        public enum VehicleType
        {
            [EnumMember(Value = "Automobile")]
            Automobile,
            [EnumMember(Value = "Motorcycle")]
            Motorcycle,
            [EnumMember(Value = "SUV")]
            SUV,
            [EnumMember(Value = "Van")]
            Van,
            [EnumMember(Value = "Pickup")]
            Pickup,
            [EnumMember(Value = "Light Truck")]
            lightTruck,
            [EnumMember(Value = "Heavy Truck")]
            heavyTruck,
            [EnumMember(Value = "Bus")]
            Bus,

        }

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
    }
}
