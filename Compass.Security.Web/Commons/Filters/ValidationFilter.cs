using System.Linq;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Compass.Security.Domain.Exceptions;

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
                    
                    foreach(var (parameter, value) in context.ModelState)
                    {
                        var rawValue = value.RawValue;
                        var attemptedValue = value.AttemptedValue;
                        viewData[parameter] = attemptedValue;
                        System.Console.WriteLine($"Parameter: {parameter}, raw: {rawValue} - value: {attemptedValue}");
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