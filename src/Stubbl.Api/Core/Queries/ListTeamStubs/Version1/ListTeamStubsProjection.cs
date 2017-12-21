namespace Stubbl.Api.Core.Queries.ListTeamStubs.Version1
{
   using System.Collections.Generic;
   using Common.Queries;
   using Shared.Version1;

   public class ListTeamStubsProjection : IProjection
   {
      public ListTeamStubsProjection(IReadOnlyCollection<Stub> stubs, Paging paging)
      {
         Stubs = stubs;
         Paging = paging;
      }

      public Paging Paging { get; }
      public IReadOnlyCollection<Stub> Stubs { get; }
   }
}