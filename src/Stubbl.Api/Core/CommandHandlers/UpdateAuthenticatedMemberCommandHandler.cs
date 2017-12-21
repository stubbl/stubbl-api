namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.UpdateAuthenticatedMember.Version1;
   using Common.CommandHandlers;
   using Data.Collections.Members;
   using Events.AuthenticatedMemberUpdated.Version1;
   using MongoDB.Driver;

   public class UpdateAuthenticatedMemberCommandHandler : ICommandHandler<UpdateAuthenticatedMemberCommand, AuthenticatedMemberUpdatedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Member> _membersCollection;

      public UpdateAuthenticatedMemberCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Member> membersCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _membersCollection = membersCollection;
      }

      public async Task<AuthenticatedMemberUpdatedEvent> HandleAsync(UpdateAuthenticatedMemberCommand command, CancellationToken cancellationToken)
      {
         var filter = Builders<Member>.Filter.Where(t => t.Id == _authenticatedMemberAccessor.AuthenticatedMember.Id);
         var update = Builders<Member>.Update.Set(t => t.Name, command.Name)
            .Set(t => t.EmailAddress, command.EmailAddress);

         await _membersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

         return new AuthenticatedMemberUpdatedEvent
         (
            command.Name,
            command.EmailAddress
         );
      }
   }
}