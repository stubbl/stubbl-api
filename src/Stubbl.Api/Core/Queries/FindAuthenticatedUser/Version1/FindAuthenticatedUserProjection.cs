namespace Stubbl.Api.Core.Queries.FindAuthenticatedUser.Version1
{
   using System.Collections.Generic;
   using CodeContrib.Queries;

   public class FindAuthenticatedUserProjection : IProjection
   {
      public FindAuthenticatedUserProjection(string id, string name, string emailAddress, IReadOnlyCollection<Team> teams)
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
