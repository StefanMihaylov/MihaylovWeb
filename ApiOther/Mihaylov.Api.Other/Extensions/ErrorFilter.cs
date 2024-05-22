using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Mihaylov.Api.Other.Extensions
{
    public class ErrorFilter : IExceptionFilter
    {
        private readonly ILoggerFactory _loggerFactory;

        public ErrorFilter(ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);

            _loggerFactory = loggerFactory;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            string action = context.RouteData.Values["action"].ToString();
            string controller = context.RouteData.Values["controller"].ToString();

            var logger = _loggerFactory.CreateLogger($"{controller}Controller");
            logger.LogError(exception, "Error in {controller}/{action}. Error: {Message}", controller, action, exception.Message);

            context.ExceptionHandled = true;

            var modelState = new ModelStateDictionary();
            if (exception is ArgumentException || exception is ArgumentNullException)
            {
                var argException = exception as ArgumentException;
                modelState.AddModelError(argException.ParamName, argException.Message);
            }
            else
            {
                modelState.AddModelError(string.Empty, exception.Message);
            }

            var response = new SerializableError(modelState);

            context.Result = new BadRequestObjectResult(response);
        }
    }
}
