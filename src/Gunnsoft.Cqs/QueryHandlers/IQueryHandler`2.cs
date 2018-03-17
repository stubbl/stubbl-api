using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;

namespace Gunnsoft.Cqs.QueryHandlers
{
    public interface IQueryHandler<in TQuery, TProjection>
        where TQuery : IQuery<TProjection>
        where TProjection : IProjection
    {
        Task<TProjection> HandleAsync(TQuery query, CancellationToken cancellationToken);
    }
}