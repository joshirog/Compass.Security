using System.Linq;
using Compass.Security.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;

namespace Compass.Security.Web.Commons.Filters
{
    public class ValidationFilter : ExceptionFilterAttribute
    {
        private IModelMetadataProvider ModelMetadataProvider { get; }

        public ValidationFilter(IModelMetadataProvider modelMetadataProvider)
        {
            ModelMetadataProvider = modelMetadataProvider;
        }
        
        public override void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ValidationException ex:
                {
                    var validationResult = new ValidationResult(ex.Errors);
                    validationResult.AddToModelState(context.ModelState, null);

                    var viewData = new ViewDataDictionary(ModelMetadataProvider, context.ModelState);
                    
                    foreach(var item in context.ModelState)
                    {
                        var parameter = item.Key;
                        var rawValue = item.Value.RawValue;
                        var attemptedValue = item.Value.AttemptedValue;
                        viewData[parameter] = attemptedValue;
                        System.Console.WriteLine($"Parameter: {parameter}, value: {attemptedValue}");
                    }
                    
                    context.Result = new ViewResult { ViewData = viewData };
                    context.ExceptionHandled = true;
                    break;
                }
                case ErrorInvalidException ex:
                    ex.Errors?.ToList().ForEach(x => context.ModelState.AddModelError("", x));
                    context.Result = new ViewResult
                    {
                        ViewData = new ViewDataDictionary(ModelMetadataProvider, context.ModelState),
                    };
                    context.ExceptionHandled = true;
                    break;
            }
        }
    }
}