using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;

namespace Gunnsoft.Cqs.EventHandlers
{
    public interface IEventHandler<in TEvent>
        where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }
}