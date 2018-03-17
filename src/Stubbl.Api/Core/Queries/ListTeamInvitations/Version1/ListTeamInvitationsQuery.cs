﻿namespace Stubbl.Api.Core.Queries.ListTeamInvitations.Version1
{
   using Gunnsoft.Cqs.Queries;
   using MongoDB.Bson;

   public class ListTeamInvitationsQuery : IQuery<ListTeamInvitationsProjection>
   {
      public ListTeamInvitationsQuery(ObjectId teamId)
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}