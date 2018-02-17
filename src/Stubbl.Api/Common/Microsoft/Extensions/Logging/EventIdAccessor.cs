namespace Microsoft.Extensions.Logging
{
   using System;

   public class EventIdAccessor : IEventIdAccessor
   {
      private readonly Lazy<EventId> _eventId;

      public EventIdAccessor()
      {
         _eventId = new Lazy<EventId>(() => new EventId());
      }

      public EventId EventId => _eventId.Value;
   }
}
