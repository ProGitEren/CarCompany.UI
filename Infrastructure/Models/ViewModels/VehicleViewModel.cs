using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.ViewModels
{
    public class VehicleViewModel
    {
        public string Id { get; set; }
        public decimal Averagefuelin { get; set; }

        public decimal Averagefuelout { get; set; }

        public int COemmission { get; set; }

        public int FuelCapacity { get; set; }

        public int MaxAllowedWeight { get; set; }

        public int MinWeight { get; set; }

        public int BaggageVolume { get; set; }

        public int DrivenKM { get; set; }

    }
}
