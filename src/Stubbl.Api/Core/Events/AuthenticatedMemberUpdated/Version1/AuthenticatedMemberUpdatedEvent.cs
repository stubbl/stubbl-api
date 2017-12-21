namespace Stubbl.Api.Core.Events.AuthenticatedMemberUpdated.Version1
{
   using Common.Events;

   public class AuthenticatedMemberUpdatedEvent : IEvent
   {
      public AuthenticatedMemberUpdatedEvent(string name, string emailAddress)
      {
         Name = name;
         EmailAddress = emailAddress;
      }

      public string EmailAddress { get; }
      public string Name { get; }
   }
}
