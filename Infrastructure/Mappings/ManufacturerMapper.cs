using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappings
{
    public static class ManufacturerMapper
    {
        public static readonly Dictionary<string, string> ManufacturerMapping = new Dictionary<string, string>
        {
            { "GM", "General Motors" },
            { "FD", "Ford" },
            { "TY", "Toyota" },
            { "BM", "BMW" },
            { "HD", "Honda" },
            { "MB", "Mercedes-Benz" },
            { "VW", "Volkswagen" },
            { "FT", "Fiat" },
            { "NS", "Nissan" },
            { "AU", "Audi" },
            { "LR", "Land Rover" },
            { "JP", "Jeep" },
            { "CH", "Chevrolet" },
            { "DS", "Dodge" },
            { "SZ", "Suzuki" },
            { "MZ", "Mazda" },
            { "HY", "Hyundai" },
            { "KI", "Kia" },
            { "PS", "Porsche" },
            { "AL", "Alfa Romeo" },
            { "MS", "Mitsubishi" },
            { "RR", "Rolls-Royce" },
            { "BN", "Bentley" },
            { "LV", "Lexus" },
            { "AC", "Acura" },
            { "IN", "Infiniti" },
            { "TS", "Tesla" },
            { "VL", "Volvo" },
            { "PE", "Peugeot" },
            { "RE", "Renault" },
            { "CI", "Citroën" },
            { "SM", "Smart" },
        };
    }
}
