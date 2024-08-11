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
    }
}
