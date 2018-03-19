using System;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;
using MongoDB.Driver;
using Stubbl.Api.Queries.Canary.Version1;

namespace Stubbl.Api.QueryHandlers
{
    public class CanaryQueryHandler : IQueryHandler<CanaryQuery, CanaryProjection>
    {
        private readonly MongoClient _mongoClient;
        private readonly MongoUrl _mongoUrl;

        public CanaryQueryHandler(MongoClient mongoClient, MongoUrl mongoUrl)
        {
            _mongoClient = mongoClient;
            _mongoUrl = mongoUrl ?? throw new ArgumentNullException(nameof(mongoUrl));
        }

        public async Task<CanaryProjection> HandleAsync(CanaryQuery query, CancellationToken cancellationToken)
        {
            var database = _mongoClient.GetDatabase(_mongoUrl.DatabaseName);
            var databaseStatus = "ok";

            try
            {
                await database.RunCommandAsync((Command<BsonDocument>) "{ping:1}",
                    cancellationToken: cancellationToken);
            }
            catch
            {
                databaseStatus = "fault";
            }

            return new CanaryProjection
            (
                databaseStatus
            );
        }
    }
}