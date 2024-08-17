using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Dto_Users
{
    public class ParamsUserDto
    {
        public int TotalItems { get; set; }
        public int PageItemCount { get; set; }


        public List<UserwithdetailDto> UserDtos { get; set; }
    }
}
