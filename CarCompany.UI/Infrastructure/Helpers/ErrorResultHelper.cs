using Infrastructure.Errors;
using Infrastructure.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public static class ErrorResultHelper
    {
        public static async Task ErrorResult(HttpResponseMessage? response)
        {


            if (!(response.StatusCode == HttpStatusCode.BadRequest))
            {
                var errormessage = await response.Content.ReadAsStringAsync();
                var ErrorObject = JsonConvert.DeserializeObject<BaseCommonResponse>(errormessage);
                throw new UIException(response.StatusCode, ErrorObject.Message);

            }
            var errormessage_ = await response.Content.ReadAsStringAsync();
            var ErrorObject_ = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(errormessage_);

            throw new UIBadRequestException(response.StatusCode, ErrorObject_.Errors);
        }
    }
}
