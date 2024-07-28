using Infrastucture.Errors;

namespace CarCompany.UI.Errors
{
    public class ApiValidationErrorResponse : BaseCommonResponse
    {
        //Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1

        public ApiValidationErrorResponse() : base(400)
        {

        }
        public IEnumerable<string> Errors { get; set; }
    }
}
