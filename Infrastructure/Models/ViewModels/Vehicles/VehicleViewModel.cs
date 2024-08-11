using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.ViewModels.Vehicles
{
    public class VehicleViewModel
    {
        public string Vin { get; set; }

        public string UserName { get; set; }
        public decimal Averagefuelin { get; set; }

        public decimal Averagefuelout { get; set; }

        public int COemmission { get; set; }

        public int FuelCapacity { get; set; }

        public int MaxAllowedWeight { get; set; }

        public int MinWeight { get; set; }

        public int BaggageVolume { get; set; }

        public int DrivenKM { get; set; }

        public int ModelId { get; set; }

        public int EngineId { get; set; }

        public string UserId { get; set; }




    }
}
