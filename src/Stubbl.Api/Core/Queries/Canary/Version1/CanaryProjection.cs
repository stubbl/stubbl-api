namespace Stubbl.Api.Core.Queries.Canary.Version1
{
   using Common.Queries;

   public class CanaryProjection : IProjection
   {
      public CanaryProjection(string database)
      {
         Database = database;
      }

      public string Database { get; }
   }
}