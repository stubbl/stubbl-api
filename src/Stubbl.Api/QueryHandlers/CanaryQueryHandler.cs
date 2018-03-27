using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Microsoft.WindowsAzure.Storage;
using MongoDB.Bson;
using MongoDB.Driver;
using Stubbl.Api.Queries.Canary.Version1;

namespace Stubbl.Api.QueryHandlers
{
    public class CanaryQueryHandler : IQueryHandler<CanaryQuery, CanaryProjection>
    {
        private readonly MongoClient _mongoClient;
        private readonly MongoUrl _mongoUrl;
        private readonly CloudStorageAccount _storageAccount;

        public CanaryQueryHandler(MongoClient mongoClient, MongoUrl mongoUrl, CloudStorageAccount storageAccount)
        {
            _mongoClient = mongoClient;
            _mongoUrl = mongoUrl ?? throw new ArgumentNullException(nameof(mongoUrl));
            _storageAccount = storageAccount;
        }

        public async Task<CanaryProjection> HandleAsync(CanaryQuery query, CancellationToken cancellationToken)
        {
            return new CanaryProjection
            (
                await VerifyMongoAsync(cancellationToken),
                await VerifyStorageAccountAsync(cancellationToken)
            );
        }

        private async Task<ComponentStatus> VerifyMongoAsync(CancellationToken cancellationToken)
        {
            ComponentStatus mongoStatus;

            var database = _mongoClient.GetDatabase(_mongoUrl.DatabaseName);

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                await database.RunCommandAsync((Command<BsonDocument>) "{ping:1}",
                    cancellationToken: cancellationToken);

                stopwatch.Stop();

                var elapsed = stopwatch.Elapsed.TotalMilliseconds;

                mongoStatus = elapsed < 1000 ? ComponentStatus.Operational : ComponentStatus.DegradedPerformance;
            }
            catch
            {
                mongoStatus = ComponentStatus.MajorOutage;
            }

            return mongoStatus;
        }

        private async Task<ComponentStatus> VerifyStorageAccountAsync(CancellationToken cancellationToken)
        {
            ComponentStatus storageAccountStatus;

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var blobClient = _storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("canary");
                await container.CreateIfNotExistsAsync();
                await container.DeleteAsync();

                stopwatch.Stop();

                var elapsed = stopwatch.Elapsed.TotalMilliseconds;

                storageAccountStatus =
                    elapsed < 1000 ? ComponentStatus.Operational : ComponentStatus.DegradedPerformance;
            }
            catch
            {
                storageAccountStatus = ComponentStatus.MajorOutage;
            }

            return storageAccountStatus;
        }
    }
}