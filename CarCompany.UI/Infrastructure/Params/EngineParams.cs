using Infrastructure.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Params
{
    public class EngineParams : PaginationParams
    {
       

        //Filter By Model
        public int? Id { get; set; }

        public string? EngineCode { get; set; }
        public string? Cylinder { get; set; }
        
       

    }
}
