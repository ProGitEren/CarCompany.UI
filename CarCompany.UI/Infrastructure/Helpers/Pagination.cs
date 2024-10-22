using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Helpers
{
    public class Pagination<T> where T : class
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PageCount  { get; set; }

        public int TotalCount { get; set; }

        public IReadOnlyList<T> Data { get; set; }
        public Pagination(int pageSize, int pageNumber, int pageCount,int totalCount, IReadOnlyList<T> data)
        {
            PageCount = pageCount;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalCount = totalCount;
            Data = data;
        }
    }

    
}
