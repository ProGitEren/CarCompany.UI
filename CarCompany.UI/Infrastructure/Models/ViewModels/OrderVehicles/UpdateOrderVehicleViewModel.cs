using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.ViewModels.OrderVehicles
{
    public class UpdateOrderVehicleViewModel
    {
        public int Id { get; set; }
        public string VehicleId { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public List<string>? ExistingImagePaths { get; set; }
        public AddFileViewModel AddFile { get; set; }
        public string? ImagePathsToDelete { get; set; }
    }
}
