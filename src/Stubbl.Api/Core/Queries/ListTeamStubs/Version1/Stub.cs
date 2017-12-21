﻿namespace Stubbl.Api.Core.Queries.ListTeamStubs.Version1
{
   using System.Collections.Generic;
   using Shared.Version1;

   public class Stub
   {
      public Stub(string id, string teamId, string name, Request request, Response response,
         IReadOnlyCollection<string> tags)
      {
         Id = id;
         TeamId = teamId;
         Name = name;
         Request = request;
         Response = response;
         Tags = tags;
      }

      public string Id { get; }
      public string Name { get; }
      public Request Request { get; }
      public Response Response { get; }
      public IReadOnlyCollection<string> Tags { get; }
      public string TeamId { get; }
   }
}