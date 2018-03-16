namespace Stubbl.Api.Core.Queries.FindTeam.Version1
{
   using CodeContrib.Queries;

   public class FindTeamProjection : IProjection
   {
      public FindTeamProjection(string id, string name)
      {
         Id = id;
         Name = name;
      }

      public string Id { get; }
      public string Name { get; }
   }
}