namespace Stubbl.Api.Common.Commands
{
   using Events;

   public interface ICommand<TEvent>
      where TEvent : IEvent
   {
   }
}