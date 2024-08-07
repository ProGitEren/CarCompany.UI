using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helpers
{
    public class ExceptionHelper
    {
        public static void HandleException(Exception ex, string? userEmail, Serilog.ILogger logger, ModelStateDictionary modelState, string actionName)
        {
            if (ex is UIException uiEx)
            {
                switch (uiEx.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        modelState.AddModelError("", $"{actionName} failed --> Not Found: {uiEx.Message}");
                        logger.Warning("{Action} failed for user {Email}: {Message}", actionName, userEmail ?? "Unknown", uiEx.Message);
                        break;

                    case HttpStatusCode.Unauthorized:
                        modelState.AddModelError("", $"{actionName} failed --> Unauthorized: {uiEx.Message}");
                        logger.Warning("{Action} failed for user {Email}: {Message}", actionName, userEmail ?? "Unknown", uiEx.Message);
                        break;
                    case HttpStatusCode.Forbidden:
                        modelState.AddModelError("", $"{actionName} failed --> Forbidden: {uiEx.Message}");
                        logger.Warning("{Action} failed for user {Email}: {Message}", actionName, userEmail ?? "Unknown", uiEx.Message);
                        break;
                    default:
                        modelState.AddModelError("", $"{actionName} failed --> Unexpected Error: {uiEx.Message}");
                        logger.Error("Unexpected error occurred during {Action} for user {Email}: {Message}", actionName, userEmail ?? "Unknown", uiEx.Message);
                        break;
                }
            }
            else if (ex is UIBadRequestException uiBadRequest)
            {

                foreach (var error in uiBadRequest.Messages)
                {
                    modelState.AddModelError("", $"{actionName} failed --> BadRequest: {error}");
                    logger.Warning("{Action} failed for user {Email}: {Message}", actionName, userEmail ?? "Unknown", error);
                }

            }

            else
            {
                modelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
                logger.Error("An unexpected error occurred during {Action} for user {Email}: {Message}", actionName, userEmail, ex.Message);
            }
        }

    }
}
