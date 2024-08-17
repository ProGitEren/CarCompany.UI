using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Params
{
    public class EngineParams
    {
        //Page size
        public int maxpagesize { get; set; } = 50;

        private int pagesize = 13;
        public int Pagesize
        {
            get => pagesize;
            set => pagesize = value > maxpagesize ? maxpagesize : value;
        }
        public int PageNumber { get; set; } = 1;

        //Filter By Model
        public int? Id { get; set; }

        public string? EngineCode { get; set; }
        public string? Cylinder { get; set; }
        
        //Sorting
        public string? Sorting { get; set; }
        //search 

        private string? _search;

        public string Search
        {
            get { return _search; }
            set
            {
                if (value is not null) { _search = value.ToLower(); }
                else { _search = value; }
            }
        }

    }
}
