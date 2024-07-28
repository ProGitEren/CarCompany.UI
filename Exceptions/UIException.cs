using System.Net;

namespace CarCompany.UI.Exceptions
{
    public class UIException : Exception
    {
        public HttpStatusCode StatusCode { get;}

        
        public UIException(HttpStatusCode statusCode, string message) : base(message) 
        {
            StatusCode = statusCode;
        }
 
    }
}
