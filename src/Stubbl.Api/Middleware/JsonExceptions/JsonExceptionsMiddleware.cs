namespace Stubbl.Api.Middleware.JsonExceptions
{
   using System;
   using System.Linq;
   using System.Net;
   using System.Reflection;
   using System.Threading.Tasks;
   using Autofac;
   using Common.ExceptionHandlers;
   using Newtonsoft.Json;
   using Core.Versioning;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.AspNetCore.Http;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.Extensions.Logging;
   using Type = System.Type;

   // TODO Circuit Breaker
   public class JsonExceptionsMiddleware
   {
      private readonly IComponentContext _componentContext;
      private readonly IEventIdAccessor _eventIdAccessor;
      private readonly IHostingEnvironment _hostingEnvironment;
      private readonly ILogger<JsonExceptionsMiddleware> _logger;
      private readonly RequestDelegate _next;
      private static readonly Type s_exceptionHandlerInterfaceType;

      static JsonExceptionsMiddleware()
      {
         s_exceptionHandlerInterfaceType = typeof(IExceptionHandler<>);
      }

      public JsonExceptionsMiddleware(RequestDelegate next, IComponentContext componentContext,
         IEventIdAccessor eventIdAccessor, IHostingEnvironment hostingEnvironment,
         ILogger<JsonExceptionsMiddleware> logger)
      {
         _next = next;
         _componentContext = componentContext;
         _eventIdAccessor = eventIdAccessor;
         _hostingEnvironment = hostingEnvironment;
         _logger = logger;
      }

      public async Task Invoke(HttpContext context)
      {
         try
         {
            await _next(context);
         }
         catch (AggregateException exception)
         {
            var innerExceptions = exception.InnerExceptions.GroupBy(e => e.GetType())
               .Select(g => g.Last())
               .ToList();

            foreach (var innerException in innerExceptions.Take(innerExceptions.Count - 1))
            {
               var innerExceptionName = innerException.GetType().FullName;

               _logger.LogWarning
               (
                  _eventIdAccessor.EventId,
                  exception,
                  "Exception {ExceptionName} thrown with message {ExceptionMessage}",
                  innerExceptionName,
                  innerException.Message
               );
            }

            await HandleException(context, innerExceptions.Last());
         }
         catch (Exception exception)
         {
            await HandleException(context, exception);
         }
      }

      private async Task HandleException(HttpContext context, Exception originalException)
      {
         if (context.Response.HasStarted)
         {
            var exceptionName = originalException.GetType().FullName;

            _logger.LogError
            (
               _eventIdAccessor.EventId,
               originalException,
               "The response has already started so exception {ExceptionName} with message {ExceptionMessage} cannot be handled",
               exceptionName, 
			   originalException.Message
            );

            return;
         }

         context.Response.Clear();

         var exceptionHandlerType = s_exceptionHandlerInterfaceType.MakeGenericType(originalException.GetType());
         dynamic exceptionHandler;

         try
         {
            exceptionHandler = _componentContext.Resolve(exceptionHandlerType);
         }
         catch (Exception exception)
         {
            var exceptionName = exception.GetType().FullName;
            var originalExceptionName = originalException.GetType().FullName;

            _logger.LogWarning
            (
               _eventIdAccessor.EventId,
               exception,
               "Exception {ExceptionName} thrown with message {ExceptionMessage} when resolving exception handler for exception {OriginalExceptionName} with message {OriginalExceptionMessage}",
               exceptionName,
               exception.Message,
               originalExceptionName,
               originalException.Message
            );

            await HandleUsingDefaultExceptionHandler(context, originalException);

            return;
         }

         var handleMethod = exceptionHandlerType.GetTypeInfo().GetMethod("HandleAsync");

         try
         {
            await handleMethod.Invoke(exceptionHandler, new object[] { context, originalException });
         }
         catch (Exception exception)
         {
            var exceptionName = exception.GetType().FullName;
            var originalExceptionName = originalException.GetType().FullName;

            _logger.LogError
            (
               _eventIdAccessor.EventId,
               exception,
               "Exception {ExceptionName} thrown with message {ExceptionMessage} when handling exception {OriginalExceptionName} with message {OriginalExceptionMessage}",
               exceptionName,
               exception.Message,
               originalExceptionName,
               originalException.Message
            );

            if (_hostingEnvironment.IsDevelopment())
            {
               var response = new Models.Exception.Version1.ExceptionResponse
               (
                  "ExceptionHandlerThrewException",
                  $"Exception {exceptionName} thrown with message {exception.Message} when handling exception {originalExceptionName} with message {originalException.Message}",
                  new Models.Exception.Version1.Exception(exception)
               );

               await context.Response.WriteJsonAsync(HttpStatusCode.InternalServerError, response, JsonConstants.JsonSerializerSettings);
            }
            else
            {
               context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
         }
      }

      private async Task HandleUsingDefaultExceptionHandler(HttpContext context, Exception originalException)
      {
         var originalExceptionName = originalException.GetType().FullName;

         if (originalException.GetType() == typeof(TimeoutException))
         {
            _logger.LogCritical
            (
               _eventIdAccessor.EventId,
               originalExceptionName,
               "Unhandled exception {ExceptionName} thrown with message {ExceptionMessage}",
               originalExceptionName,
               originalException.Message
            );
         }
         else
         {
            _logger.LogError
            (
               _eventIdAccessor.EventId,
               originalException,
               "Unhandled exception {ExceptionName} thrown with message {ExceptionMessage}",
               originalExceptionName,
               originalException.Message
            );
         }

         var version = context.GetRequestedApiVersion()?.MajorVersion ?? Versions.Latest;

         IDefaultExceptionHandler defaultExceptionHandler;

         try
         {
            defaultExceptionHandler = _componentContext.ResolveVersioned<IDefaultExceptionHandler>(version);
         }
         catch (Exception exception)
         {
            var exceptionName = exception.GetType().FullName;

            _logger.LogError
            (
               _eventIdAccessor.EventId,
               exception,
               "Exception {ExceptionName} thrown with message {ExceptionMessage} when resolving default exception handler for exception {OriginalExceptionName} with message {OriginalExceptionMessage} for version {Version}",
               exceptionName,
               exception.Message,
               originalExceptionName,
               originalException.Message,
               version
            );

            if (_hostingEnvironment.IsDevelopment())
            {
               var response = new Models.Exception.Version1.ExceptionResponse
               (
                  "DefaultExceptionHandlerNotRegistered",
                  $"Exception {exceptionName} thrown with message {exception.Message} when resolving default exception handler for exception {originalExceptionName} with message {originalException.Message} for version {version}",
                  new Models.Exception.Version1.Exception(exception)
               );

               await context.Response.WriteJsonAsync(HttpStatusCode.InternalServerError, response, JsonConstants.JsonSerializerSettings);
            }
            else
            {
               context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return;
         }

         try
         {
            await defaultExceptionHandler.HandleAsync(context, originalException);
         }
         catch (Exception exception)
         {
            var exceptionName = exception.GetType().FullName;

            _logger.LogError
            (
               _eventIdAccessor.EventId,
               exception,
               "Exception {ExceptionName} thrown with message {ExceptionMessage} when handling exception {OriginalExceptionName} with message {OriginalExceptionMessage}",
               exceptionName,
               exception.Message,
               originalExceptionName,
               originalException.Message
            );

            if (_hostingEnvironment.IsDevelopment())
            {
               var response = new Models.Exception.Version1.ExceptionResponse
               (
                  "DefaultExceptionHandlerThrewException",
                  $"Exception {exceptionName} thrown with message {exception.Message} when handling exception {originalExceptionName} with message {originalException.Message}",
                  new Models.Exception.Version1.Exception(exception)
               );

               await context.Response.WriteJsonAsync(HttpStatusCode.InternalServerError, response, JsonConstants.JsonSerializerSettings);
            }
            else
            {
               context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
         }
      }
   }
}