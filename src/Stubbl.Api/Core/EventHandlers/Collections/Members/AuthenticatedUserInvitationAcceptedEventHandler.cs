namespace Stubbl.Api.Core.EventHandlers.Collections.Members
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using CodeContrib.EventHandlers;
   using Data.Collections.Invitations;
   using Data.Collections.Members;
   using Events.AuthenticatedUserInvitationAccepted.Version1;
   using MongoDB.Driver;
   using Team = Data.Collections.Members.Team;

   public class AuthenticatedUserInvitationAcceptedEventHandler : IEventHandler<AuthenticatedUserInvitationAcceptedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;
      private readonly IMongoCollection<Member> _membersCollection;

      public AuthenticatedUserInvitationAcceptedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Invitation> invitationsCollection, IMongoCollection<Member> membersCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _invitationsCollection = invitationsCollection;
         _membersCollection = membersCollection;
      }

      public async Task HandleAsync(AuthenticatedUserInvitationAcceptedEvent @event, CancellationToken cancellationToken)
      {
         var invitation = await _invitationsCollection.Find(i => i.Id == @event.InvitationId)
            .SingleAsync(cancellationToken);

         var team = new Team
         {
            Id = invitation.Team.Id,
            Name = invitation.Team.Name,
            Role = new Data.Collections.Members.Role
            {
               Id = invitation.Role.Id,
               Name = invitation.Role.Name
            }
         };

         var filter = Builders<Member>.Filter.Where(m => m.Id == _authenticatedUserAccessor.AuthenticatedUser.Id);
         var update = Builders<Member>.Update.Push(m => m.Teams, team);

         await _membersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

      }
   }
}
