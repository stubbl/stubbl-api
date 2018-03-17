using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Gunnsoft.Cqs.Commands;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stubbl.Api.Commands.CreateTeamLog.Version1;
using Stubbl.Api.Commands.Shared.Version1;
using Stubbl.Api.Options;
using Stubbl.Api.Queries.StubTester.Version1;
using Request = Stubbl.Api.Queries.StubTester.Version1.Request;
using Response = Stubbl.Api.Commands.CreateTeamLog.Version1.Response;

namespace Stubbl.Api.Middleware.Stub
{
    public class StubMiddleware
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly RequestDelegate _next;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly StubblApiOptions _stubblApiOptions;

        public StubMiddleware(RequestDelegate next, ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher, IOptions<StubblApiOptions> stubblApiOptions)
        {
            _next = next;
            _stubblApiOptions = stubblApiOptions.Value;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public async Task Invoke(HttpContext context)
        {
            var cancellationToken = context?.RequestAborted ?? default(CancellationToken);
            var teamId = ParseTeamId(context.Request.Host);

            if (teamId == null)
            {
                await _next(context);

                return;
            }

            var query = new StubTesterQuery
            (
                teamId.Value,
                new Request
                (
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.Query.Count,
                    context.Request.Headers.Count
                )
            );

            var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            if (projection == null || !projection.Stubs.Any())
            {
                await NotFound(context, teamId.Value, cancellationToken);

                return;
            }

            var requestStream = context.Request.Body;
            string requestBody;

            using (var streamReader = new StreamReader(requestStream))
            {
                requestBody = streamReader.ReadToEnd();
            }

            var stubs = projection.Stubs.Where(s =>
                {
                    if (s.Request.Headers != null)
                    {
                        if (s.Request.Headers.Any(requestHeader =>
                            !context.Request.Headers.ContainsKey(requestHeader.Key) || !string.Equals(
                                context.Request.Headers[requestHeader.Key], requestHeader.Value,
                                StringComparison.OrdinalIgnoreCase)))
                        {
                            return false;
                        }
                    }

                    if (string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase) ||
                        s.Request.BodyTokens == null)
                    {
                        return true;
                    }

                    if (string.IsNullOrWhiteSpace(requestBody))
                    {
                        return false;
                    }

                    JObject request = null;
                    var isXml = false;

                    try
                    {
                        var xElement = XElement.Load(requestBody);
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
                            request = JObject.Parse(requestBody);
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

            if (stubs.Count > 1)
            {
                var response = new
                {
                    Code = "500",
                    Message = $"{stubs.Count} stubs were found for this request.",
                    Stubs = stubs.Select(x => new
                    {
                        x.StubId
                    })
                };

                var responseJson = JsonConvert.SerializeObject(response, Formatting.Indented,
                    JsonConstants.JsonSerializerSettings);

                var response500 = new Response
                (
                    500,
                    responseJson,
                    null
                );

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = response500.HttpStatusCode;
                await context.Response.WriteAsync(responseJson, cancellationToken);

                await UpdateLogs(context, teamId.Value, null, response500, cancellationToken);

                return;
            }

            var stub = stubs.SingleOrDefault();

            if (stub == null)
            {
                await NotFound(context, teamId.Value, cancellationToken);

                return;
            }

            context.Response.StatusCode = stub.Response.HttpStatusCode;

            if (stub.Response.Headers != null)
            {
                foreach (var responseHeader in stub.Response.Headers)
                {
                    context.Response.Headers.Add(responseHeader.Key, responseHeader.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(stub.Response.Body))
            {
                await context.Response.WriteAsync(stub.Response.Body, cancellationToken);
            }

            var stubResponse = new Response
            (
                stub.Response.HttpStatusCode,
                stub.Response.Body,
                stub.Response.Headers?.Select(h => new Header(h.Key, h.Value)).ToList()
            );

            await UpdateLogs(context, teamId.Value, new[] {ObjectId.Parse(stub.StubId)}, stubResponse,
                cancellationToken);
        }

        private async Task NotFound(HttpContext context, ObjectId teamId, CancellationToken cancellationToken)
        {
            var response = new
            {
                Code = "404",
                Message = "0 stubs were found for this request."
            };

            var responseJson =
                JsonConvert.SerializeObject(response, Formatting.Indented, JsonConstants.JsonSerializerSettings);

            var response404 = new Response
            (
                404,
                responseJson,
                null
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response404.HttpStatusCode;
            await context.Response.WriteAsync(responseJson, cancellationToken);

            await UpdateLogs(context, teamId, null, response404, cancellationToken);
        }

        private ObjectId? ParseTeamId(HostString host)
        {
            var subdomainMatch = Regex.Match(host.Value, $"^([^\\.]*)\\.{_stubblApiOptions.Host}$");

            if (!subdomainMatch.Success)
            {
                return null;
            }

            var subdomain = subdomainMatch.Groups[1].Value;

            if (!ObjectId.TryParse(subdomain, out var teamId))
            {
                return null;
            }

            return teamId;
        }

        private async Task UpdateLogs(HttpContext context, ObjectId teamId, IReadOnlyCollection<ObjectId> stubIds,
            Response response, CancellationToken cancellationToken)
        {
            var requestStream = context.Request.Body;
            string requestBody;

            using (var streamReader = new StreamReader(requestStream))
            {
                requestBody = streamReader.ReadToEnd();
            }

            var command = new CreateTeamLogCommand
            (
                teamId,
                stubIds,
                new Commands.CreateTeamLog.Version1.Request
                (
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.Query.Select(q => new QueryStringParameter(q.Key, q.Value)).ToList(),
                    requestBody,
                    context.Request.Headers.Select(h => new Header(h.Key, h.Value)).ToList()
                ),
                response
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
    }
}