namespace Stubbl.Api.Core.Queries.ListTeamLogs.Version1
{
   using System;
   using System.Collections.Generic;
   using Shared.Version1;

   public class Log
   {
      public Log(string id, string teamId, IReadOnlyCollection<string> stubIds, RequestLog request, ResponseLog response, DateTime loggedAt)
      {
         Id = id;
         TeamId = teamId;
         StubIds = stubIds;
         Request = request;
         Response = response;
         LoggedAt = loggedAt;
      }

      public string Id { get; }
      public DateTime LoggedAt { get; }
      public RequestLog Request { get; }
      public ResponseLog Response { get; }
      public IReadOnlyCollection<string> StubIds { get; }
      public string TeamId { get; }
   }
}