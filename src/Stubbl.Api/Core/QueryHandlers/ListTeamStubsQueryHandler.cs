﻿namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Common.QueryHandlers;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;
   using Queries.ListTeamStubs.Version1;
   using Queries.Shared.Version1;
   using Stub = Data.Collections.Stubs.Stub;

   public class ListTeamStubsQueryHandler : IQueryHandler<ListTeamStubsQuery, ListTeamStubsProjection>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Stub> _stubsCollection;

      public ListTeamStubsQueryHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Stub> stubsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _stubsCollection = stubsCollection;
      }

      public async Task<ListTeamStubsProjection> HandleAsync(ListTeamStubsQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedMemberAccessor.AuthenticatedMember.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               query.TeamId
            );
         }

         var filter = Builders<Stub>.Filter.Where(s => s.TeamId == query.TeamId);

         if (!string.IsNullOrWhiteSpace(query.Search))
         {
            filter = Builders<Stub>.Filter.And(filter, Builders<Stub>.Filter.Regex(s => s.Name, query.Search));
            filter = Builders<Stub>.Filter.Or(filter, Builders<Stub>.Filter.Regex(s => s.Request.Path, query.Search));
            filter = Builders<Stub>.Filter.Or(filter,Builders<Stub>.Filter.Regex(s => s.Tags, query.Search));
         }

         var totalCount = await _stubsCollection.Find(filter).CountAsync(cancellationToken);
         var stubs = await _stubsCollection.Find(filter)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Limit(query.PageSize)
            .Project(s => new Queries.ListTeamStubs.Version1.Stub
            (
               s.Id.ToString(),
               s.TeamId.ToString(),
               s.Name,
               new Request
               (
                  s.Request.HttpMethod,
                  s.Request.Path,
                  s.Request.QueryStringParameters == null ? null : s.Request.QueryStringParameters.Select(qcc => new QueryStringParameter(qcc.Key, qcc.Value)).ToList(),
                  s.Request.BodyTokens == null ? null : s.Request.BodyTokens.Select(bt => new BodyToken(bt.Path, bt.Type, bt.Value)).ToList(),
                  s.Request.Headers == null ? null : s.Request.Headers.Select(h => new Header(h.Key, h.Value)).ToList()
               ),
               new Response
               (
                  s.Response.HttpStatusCode,
                  s.Response.Body,
                  s.Request.Headers == null ? null : s.Request.Headers.Select(h => new Header(h.Key, h.Value)).ToList()
               ),
               s.Tags
            ))
            .ToListAsync(cancellationToken);

         return new ListTeamStubsProjection
         (
            stubs,
            new Paging
            (
               query.PageNumber,
               query.PageSize,
               totalCount
            )
         );
      }
   }
}