namespace Stubbl.Api.Core.Queries.ListTeams.Version1
{
   using System.Collections.Generic;
   using Gunnsoft.Cqs.Queries;

   public class ListTeamsProjection : IProjection
   {
      public ListTeamsProjection(IReadOnlyCollection<Team> teams)
      {
         Teams = teams;
      }

      public IReadOnlyCollection<Team> Teams { get; }
   }
}