using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Dto_Vehicles
{
    public class RegisterVehicleDto
    {

        public decimal Averagefuelin { get; set; }

        public string UserName { get; set; }
        public decimal Averagefuelout { get; set; }

        public int COemmission { get; set; }

        public int FuelCapacity { get; set; }

        public int MaxAllowedWeight { get; set; }

        public int MinWeight { get; set; }

        public int BaggageVolume { get; set; }

        public int DrivenKM { get; set; }

        public int? ModelId { get; set; }

    }
}
