using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Stubbl.Api.Data
{
    public interface IMongoMigration
    {
        ObjectId Id { get; }
        string Name { get; }

        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}