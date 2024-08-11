using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappings
{
    public static class SecurityCodeMapper
    {
        public static readonly List<string> SecurityCodes = 
            "01234566789X".Select(c => c.ToString()).ToList();
    }
}
