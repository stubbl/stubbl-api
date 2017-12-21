namespace Stubbl.Api.Core.Queries.ListTeamMembers.Version1
{
   using System.Collections.Generic;
   using Common.Queries;
   using Shared.Version1;

   public class ListTeamMembersProjection : IProjection
   {
      public ListTeamMembersProjection(IReadOnlyCollection<Member> members, Paging paging)
      {
         Members = members;
         Paging = paging;
      }

      public IReadOnlyCollection<Member> Members { get; }
      public Paging Paging { get; }
   }
}