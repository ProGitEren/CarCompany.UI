using Infrastructure.Models.ViewModels.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Comparers
{
    public class VehicleEngineIdComparer : IEqualityComparer<VehicleViewModel>
    {
        public bool Equals(VehicleViewModel x, VehicleViewModel y)
        {
            if (x == null || y == null)
                return false;

            return x.EngineId == y.EngineId;
        }

        public int GetHashCode(VehicleViewModel obj)
        {
            if (obj == null)
                return 0;

            return obj.EngineId.GetHashCode();
        }
    }
}
