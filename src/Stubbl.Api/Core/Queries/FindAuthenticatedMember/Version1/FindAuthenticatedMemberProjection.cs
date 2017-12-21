namespace Stubbl.Api.Core.Queries.FindAuthenticatedMember.Version1
{
   using System.Collections.Generic;
   using Common.Queries;

   public class FindAuthenticatedMemberProjection : IProjection
   {
      public FindAuthenticatedMemberProjection(string id, string name, string emailAddress, IReadOnlyCollection<Team> teams)
      {
         Id = id;
         Name = name;
         EmailAddress = emailAddress;
         Teams = teams;
      }

      public string Id { get; }
      public string Name { get; }
      public string EmailAddress { get; }
      public IReadOnlyCollection<Team> Teams { get; }
   }
}
