using Infrastructure.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public static class PaginationBuilderHelper <T> where T : PaginationParams
    { 
        public static string QueryString(T param)
        {
            var builder = new StringBuilder();
            builder.Append($"?PageNumber={param.PageNumber}");
            builder.Append($"&Pagesize={param.Pagesize}");

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(param);
                if (value != null)
                {
                    var name = property.Name;
                    var stringValue = value.ToString();

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        if (name == nameof(param.PageNumber) || name == nameof(param.Pagesize) || name == nameof(param.maxpagesize))
                            continue; // Skip these as they are already appended at the start.

                        // Check if the property type is nullable (like int?) or a simple string
                        if (property.PropertyType == typeof(int?) && (int?)value != null)
                        {
                            builder.Append($"&{name}={value}");
                        }
                        else if (property.PropertyType == typeof(string) && !string.IsNullOrEmpty(stringValue))
                        {
                            builder.Append($"&{name}={value}");
                        }
                    }
                }
            }
            return builder.ToString();

        }
    }
}
