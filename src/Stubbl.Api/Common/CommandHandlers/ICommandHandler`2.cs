namespace Stubbl.Api.Common.CommandHandlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Commands;
   using Events;

   public interface ICommandHandler<in TCommand, TEvent>
      where TCommand : ICommand<TEvent>
      where TEvent : IEvent
   {
      Task<TEvent> HandleAsync(TCommand command, CancellationToken cancellationToken);
   }
}