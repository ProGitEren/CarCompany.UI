using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public static class IndexPageErrorsHelper
    {
        public static void TempDataErrors(ModelStateDictionary ModelState, ITempDataDictionary TempData) 
        {
            var errors = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            TempData["Errors"] = JsonConvert.SerializeObject(errors);

        }

        public static void ShowTempDataErrors(ModelStateDictionary ModelState, ITempDataDictionary TempData)
        {
            if (TempData["Errors"] != null)
            {
                List<string>? errors = JsonConvert.DeserializeObject<List<string>>(TempData["Errors"].ToString());
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
        }

    }
}
