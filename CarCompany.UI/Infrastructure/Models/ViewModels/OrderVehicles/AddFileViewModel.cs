using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.ViewModels.OrderVehicles
{
    public class AddFileViewModel
    
    {
        public int Id { get; set; }
        public IFormFile Picture { get; set; }

    }
}
