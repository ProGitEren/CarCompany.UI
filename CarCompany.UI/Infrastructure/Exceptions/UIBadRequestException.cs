using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public class UIBadRequestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public IEnumerable<string> Messages { get; }


        public UIBadRequestException(HttpStatusCode statusCode, IEnumerable<string> messages)
        {
            StatusCode = statusCode;
            Messages = messages;
        }

    }
}
