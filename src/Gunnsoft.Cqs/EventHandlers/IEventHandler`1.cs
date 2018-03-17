namespace Gunnsoft.Cqs.EventHandlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Events;

   public interface IEventHandler<in TEvent>
      where TEvent : IEvent
   {
      Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
   }
}