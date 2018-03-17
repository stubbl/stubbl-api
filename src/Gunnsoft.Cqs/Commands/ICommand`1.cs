namespace Gunnsoft.Cqs.Commands
{
   using Events;

   public interface ICommand<TEvent>
      where TEvent : IEvent
   {
   }
}