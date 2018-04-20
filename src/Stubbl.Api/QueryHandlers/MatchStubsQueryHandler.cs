using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Queries.MatchStubs.Version1;
using Stubbl.Api.Queries.Shared.Version1;
using Request = Stubbl.Api.Queries.Shared.Version1.Request;
using Stub = Stubbl.Api.Data.Collections.Stubs.Stub;

namespace Stubbl.Api.QueryHandlers
{
    public class MatchStubsQueryHandler : IQueryHandler<MatchStubsQuery, MatchStubsProjection>
    {
        private readonly IMongoCollection<Stub> _stubsCollection;
        private readonly IMongoCollection<Team> _teamsCollection;

        public MatchStubsQueryHandler(IMongoCollection<Stub> stubsCollection, IMongoCollection<Team> teamsCollection)
        {
            _stubsCollection = stubsCollection;
            _teamsCollection = teamsCollection;
        }

        public async Task<MatchStubsProjection> HandleAsync(MatchStubsQuery query, CancellationToken cancellationToken)
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
                         && s.Request.QueryStringParameters.Count >= query.Request.QueryStringParameters.Count
                         && s.Request.Headers.Count >= query.Request.Headers.Count
                )
                .ToListAsync(cancellationToken);

            if (!stubs.Any())
            {
                return new MatchStubsProjection
                (
                    new Queries.MatchStubs.Version1.Stub[0]
                );
            }

            stubs = stubs.Where(s =>
                {
                    if (s.Request.QueryStringParameters != null)
                    {
                        if (s.Request.QueryStringParameters.Any(qsp =>
                            !query.Request.QueryStringParameters.ContainsKey(qsp.Key) || !string.Equals(
                                query.Request.QueryStringParameters[qsp.Key], qsp.Value,
                                StringComparison.OrdinalIgnoreCase)))
                        {
                            return false;
                        }
                    }

                    if (s.Request.Headers != null)
                    {
                        if (s.Request.Headers.Any(h =>
                            !query.Request.Headers.ContainsKey(h.Key) || !string.Equals(
                                query.Request.Headers[h.Key], h.Value,
                                StringComparison.OrdinalIgnoreCase)))
                        {
                            return false;
                        }
                    }

                    if (string.Equals(query.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase) ||
                        s.Request.BodyTokens == null)
                    {
                        return true;
                    }

                    if (string.IsNullOrWhiteSpace(query.Request.Body))
                    {
                        return false;
                    }

                    JObject request = null;
                    var isXml = false;

                    try
                    {
                        var xElement = XElement.Load(query.Request.Body);
                        var json = JsonConvert.SerializeXNode(xElement);
                        request = JObject.Parse(json);

                        isXml = true;
                    }
                    catch
                    {
                        // Do nothing
                    }

                    if (request == null)
                    {
                        try
                        {
                            request = JObject.Parse(query.Request.Body);
                        }
                        catch (Exception)
                        {
                            // Do nothing
                        }
                    }

                    if (request == null)
                    {
                        return false;
                    }

                    foreach (var bodyToken in s.Request.BodyTokens)
                    {
                        JToken token;

                        try
                        {
                            token = request.SelectToken(bodyToken.Path);
                        }
                        catch
                        {
                            return false;
                        }

                        if (token == null || !isXml && token.Type != bodyToken.Type ||
                            !string.Equals(token.Value<string>(), bodyToken.ToString(),
                                StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                    }

                    return true;
                })
                .ToList();

            return new MatchStubsProjection
            (
                stubs.Select(s => new Queries.MatchStubs.Version1.Stub
                    (
                        s.Id.ToString(),
                        new Request
                        (
                            s.Request.HttpMethod,
                            s.Request.Path,
                            s.Request.QueryStringParameters.Select(qsp => new QueryStringParameter(qsp.Key, qsp.Value))
                                .ToList(),
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