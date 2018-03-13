namespace Stubbl.Api.Core.Events.AuthenticatedUserUpdated.Version1
{
   using Common.Events;

   public class AuthenticatedUserUpdatedEvent : IEvent
   {
      public AuthenticatedUserUpdatedEvent(string name, string emailAddress)
      {
         Name = name;
         EmailAddress = emailAddress;
      }

      public string EmailAddress { get; }
      public string Name { get; }
   }
}
