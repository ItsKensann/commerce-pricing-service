using commercepricingservice.Models.V1;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace commercepricingservice.Middleware
{
    /// <summary>
    /// Bad Request Response
    /// </summary>
    public class BadRequestResponse : ApiResponse
    {
        /// <summary>
        /// Main constructor
        /// </summary>
        public BadRequestResponse() { }

        /// <summary>
        /// Constructor with context
        /// </summary>
        /// <param name="context"></param>
        public BadRequestResponse(ActionContext context)
        {
            TransactionId = context.HttpContext.Items["transactionId"]!.ToString()!;
            OperationName = context.HttpContext.Items["operationName"]!.ToString()!;
            StatusCode = (int)HttpStatusCode.BadRequest;
            StatusDescription = nameof(HttpStatusCode.BadRequest);
            Errors = GenerateErrors(context);
        }

        private static List<string> GenerateErrors(ActionContext context) 
        {
            List<string> errorList = new List<string>();

            foreach (var keyModelStatePair in context.ModelState)
            {
                var errors = keyModelStatePair.Value.Errors;

                if (errors == null || errors.Count <= 0) continue;

                errorList.AddRange(errors.Select(modelError => modelError.ErrorMessage));
            }

            return errorList;
        }
    }
}
