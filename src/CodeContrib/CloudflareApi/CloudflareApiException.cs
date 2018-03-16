namespace CodeContrib.CloudflareApi
{
   using System;
   using System.Collections.Generic;

   public class CloudflareApiException : Exception
   {
      public CloudflareApiException()
         : this(new CloudflareApiError[0])
      {
      }

      public CloudflareApiException(IReadOnlyCollection<CloudflareApiError> errors)
      {
         Errors = errors;
      }

      public IReadOnlyCollection<CloudflareApiError> Errors { get; }
   }
}
