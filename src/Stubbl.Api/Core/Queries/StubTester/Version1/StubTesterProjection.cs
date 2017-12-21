﻿namespace Stubbl.Api.Core.Queries.StubTester.Version1
{
   using System.Collections.Generic;
   using Common.Queries;

   public class StubTesterProjection : IProjection
   {
      public StubTesterProjection(IReadOnlyCollection<Stub> stubs)
      {
         Stubs = stubs;
      }

      public IReadOnlyCollection<Stub> Stubs { get; }
   }
}