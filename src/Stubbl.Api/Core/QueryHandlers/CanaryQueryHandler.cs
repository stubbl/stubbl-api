namespace Stubbl.Api.Core.QueryHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Gunnsoft.Cqs.QueryHandlers;
    using Data;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Queries.Canary.Version1;

    public class CanaryQueryHandler : IQueryHandler<CanaryQuery, CanaryProjection>
    {
        private readonly MongoClient _mongoClient;
        private readonly MongoUrl _mongoUrl;

        public CanaryQueryHandler(MongoClient mongoClient, MongoUrl mongoUrl)
        {
            _mongoClient = mongoClient;
            _mongoUrl = mongoUrl ?? throw new System.ArgumentNullException(nameof(mongoUrl));
        }

        public async Task<CanaryProjection> HandleAsync(CanaryQuery query, CancellationToken cancellationToken)
        {
            var database = _mongoClient.GetDatabase(_mongoUrl.DatabaseName);
            var databaseStatus = "ok";

            try
            {
                await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);
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