﻿namespace Stubbl.Api.Common.CloudflareApi
{
   public class DeleteRequest
   {
      public DeleteRequest(string pathAndQueryString)
      {
         PathAndQueryString = pathAndQueryString;
      }

      public string PathAndQueryString { get; }
   }
}
