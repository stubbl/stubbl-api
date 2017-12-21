namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Common.QueryHandlers;
   using Data.Collections.Teams;
   using MongoDB.Driver;
   using Queries.Shared.Version1;
   using Queries.StubTester.Version1;
   using Request = Queries.Shared.Version1.Request;
   using Stub = Data.Collections.Stubs.Stub;

   public class StubTesterQueryHandler : IQueryHandler<StubTesterQuery, StubTesterProjection>
   {
      private readonly IMongoCollection<Stub> _stubsCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public StubTesterQueryHandler(IMongoCollection<Stub> stubsCollection, IMongoCollection<Team> teamsCollection)
      {
         _stubsCollection = stubsCollection;
         _teamsCollection = teamsCollection;
      }
      
      public async Task<StubTesterProjection> HandleAsync(StubTesterQuery query, CancellationToken cancellationToken)
      {
         var team = await _teamsCollection.Find(t => t.Id == query.TeamId)
               .Project(t => new
               {
                  t.Id
               })
               .SingleOrDefaultAsync(cancellationToken);

         if (team == null)
         {
            return null;
         }

         var stubs = await _stubsCollection.Find
            (
               s => s.TeamId == team.Id
               && s.Request.Path.ToLower() == query.Request.Path.ToLower()
               && s.Request.HttpMethod.ToLower() == query.Request.HttpMethod.ToLower()
               && s.Request.QueryStringParameters.Count == query.Request.QueryStringParameterCount
               && s.Request.Headers.Count <= query.Request.HeaderCount
            )
            .ToListAsync(cancellationToken);

         return new StubTesterProjection
         (
            stubs.Select(s => new Queries.StubTester.Version1.Stub
               (
                  s.Id.ToString(),
                  new Request
                  (
                     s.Request.HttpMethod,
                     s.Request.Path,
                     s.Request.QueryStringParameters.Select(qcc => new QueryStringParameter(qcc.Key, qcc.Value)).ToList(),
                     s.Request.BodyTokens.Select(bt => new BodyToken(bt.Path, bt.Type, bt.Value)).ToList(),
                     s.Request.Headers.Select(h => new Header(h.Key, h.Value)).ToList()
                  ),
                  new Response
                  (
                     s.Response.HttpStatusCode,
                     s.Response.Body,
                     s.Response.Headers.Select(h => new Header(h.Key, h.Value)).ToList()
                  )
               ))
               .ToList()
         );
      }
   }
}