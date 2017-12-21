﻿namespace Stubbl.Api.Models.Exception.Version1
{
   using Error.Version1;

   public class ExceptionResponse : ErrorResponse
   {
      public ExceptionResponse(Exception exception)
      {
         Exception = exception;
      }

      public ExceptionResponse(string errorCode, string errorMessage, Exception exception)
         : base(errorCode, errorMessage)
      {
         Exception = exception;
      }

      public Exception Exception { get; }
   }
}