using Infrastructure.Models.ViewModels.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.ViewModels.OrderVehicles
{
    public class OrderVehicleDetailsViewModel
    {
        public OrderVehicleViewModel OrderVehicle { get; set; }

        public VehicleViewModel Vehicle { get; set; }
    }
}
