using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Award.Web.Common.Utils
{
    public class ValidationError
    {
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }

        public static IEnumerable<ValidationError> GetValidationErrorListFromModelState(ModelStateDictionary modelState)
        {
            IEnumerable<ValidationError> validationErrorList = null;

            if (modelState != null)
            {
                validationErrorList =  modelState.Keys.SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)));
            }

            return validationErrorList;
        }
    }
}
