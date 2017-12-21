namespace Stubbl.Api.Core.Events.TeamCreated.Version1
{
   using System.Collections.Generic;
   using Common.Events;
   using MongoDB.Bson;

   public class TeamCreatedEvent : IEvent
   {
      public TeamCreatedEvent(ObjectId teamId, string name, IReadOnlyCollection<Member> members, IReadOnlyCollection<Role> roles)
      {
         TeamId = teamId;
         Name = name;
         Members = members;
         Roles = roles;
      }

      public string Name { get; }
      public IReadOnlyCollection<Member> Members { get; }
      public IReadOnlyCollection<Role> Roles { get; }
      public ObjectId TeamId { get; }
   }
}
