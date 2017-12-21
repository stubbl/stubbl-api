namespace Stubbl.Api.Common.Events
{
   using System.Threading;
   using System.Threading.Tasks;

   public interface IEventDispatcher
   {
      Task DispatchAsync(IEvent @event, CancellationToken cancellationToken);
   }
}