namespace Stubbl.Api.Tests.Integration.ExceptionHandlers.Version1
{
   using System;
   using System.Collections;
   using System.Net;
   using Core.Authentication;
   using Core.Exceptions.AdministratorRoleNotFound.Version1;
   using Core.Exceptions.AuthenticatedMemberNotFound.Version1;
   using Core.Exceptions.InvitationAlreadyUsed.Version1;
   using Core.Exceptions.InvitationNotFound.Version1;
   using Core.Exceptions.LogNotFound.Version1;
   using Core.Exceptions.MemberAlreadyAddedToTeam.Version1;
   using Core.Exceptions.MemberAlreadyInvitedToTeam.Version1;
   using Core.Exceptions.MemberCannotBeRemovedFromTeam.Version1;
   using Core.Exceptions.MemberCannotManageInvitations.Version1;
   using Core.Exceptions.MemberCannotManageMembers.Version1;
   using Core.Exceptions.MemberCannotManageRoles.Version1;
   using Core.Exceptions.MemberCannotManageStubs.Version1;
   using Core.Exceptions.MemberCannotManageTeams.Version1;
   using Core.Exceptions.MemberNotAddedToTeam.Version1;
   using Core.Exceptions.MemberNotFound.Version1;
   using Core.Exceptions.MemberNotInvitedToTeam.Version1;
   using Core.Exceptions.RoleAlreadyExists.Version1;
   using Core.Exceptions.RoleCannotBeUpdated.Version1;
   using Core.Exceptions.RoleNotFound.Version1;
   using Core.Exceptions.StubNotFound.Version1;
   using Core.Exceptions.TeamNotFound.Version1;
   using Core.Exceptions.UserRoleNotFound.Version1;
   using Models.AuthenticatedMemberNotFound.Version1;
   using Models.Error.Version1;
   using Models.InvitationAlreadyUsed.Version1;
   using Models.InvitationNotFound.Version1;
   using Models.LogNotFound.Version1;
   using Models.MemberAlreadyAddedToTeam.Version1;
   using Models.MemberAlreadyInvitedToTeam.Version1;
   using Models.MemberCannotBeRemovedFromTeam.Version1;
   using Models.MemberCannotManageInvitations.Version1;
   using Models.MemberCannotManageMembers.Version1;
   using Models.MemberCannotManageRoles.Version1;
   using Models.MemberCannotManageStubs.Version1;
   using Models.MemberCannotManageTeams.Version1;
   using Models.MemberNotAddedToTeam.Version1;
   using Models.MemberNotFound.Version1;
   using Models.MemberNotInvitedToTeam.Version1;
   using Models.RoleAlreadyExists.Version1;
   using Models.RoleCannotBeUpdated.Version1;
   using Models.RoleNotFound.Version1;
   using Models.StubNotFound.Version1;
   using Models.TeamNotFound.Version1;
   using Models.UnknownIdentityId.Version1;
   using MongoDB.Bson;

   public class ExceptionHandlerTestFixtureData : IEnumerable
   {
      public IEnumerator GetEnumerator()
      {
         yield return new object[] { new AdministratorRoleNotFoundException(), HttpStatusCode.InternalServerError, new ErrorResponse() };
         yield return new object[] { new AuthenticatedMemberNotFoundException(null), HttpStatusCode.Unauthorized, new AuthenticatedMemberNotFoundResponse() };
         yield return new object[] { new Exception(), HttpStatusCode.InternalServerError, new ErrorResponse() };
         yield return new object[] { new InvitationAlreadyUsedException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new InvitationAlreadyUsedResponse() };
         yield return new object[] { new InvitationNotFoundException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new InvitationNotFoundResponse() };
         yield return new object[] { new LogNotFoundException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new LogNotFoundResponse() };
         yield return new object[] { new MemberAlreadyAddedToTeamException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new MemberAlreadyAddedToTeamResponse() };
         yield return new object[] { new MemberAlreadyInvitedToTeamException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new MemberAlreadyInvitedToTeamResponse() };
         yield return new object[] { new MemberCannotBeRemovedFromTeamException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new MemberCannotBeRemovedFromTeamResponse() };
         yield return new object[] { new MemberCannotManageInvitationsException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Forbidden, new MemberCannotManageInvitationsResponse() };
         yield return new object[] { new MemberCannotManageMembersException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Forbidden, new MemberCannotManageMembersResponse() };
         yield return new object[] { new MemberCannotManageRolesException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Forbidden, new MemberCannotManageRolesResponse() };
         yield return new object[] { new MemberCannotManageStubsException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Forbidden, new MemberCannotManageStubsResponse() };
         yield return new object[] { new MemberCannotManageTeamsException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Forbidden, new MemberCannotManageTeamsResponse() };
         yield return new object[] { new MemberNotAddedToTeamException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Forbidden, new MemberNotAddedToTeamResponse() };
         yield return new object[] { new MemberNotFoundException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new MemberNotFoundResponse() };
         yield return new object[] { new MemberNotInvitedToTeamException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new MemberNotInvitedToTeamResponse() };
         yield return new object[] { new RoleAlreadyExistsException(Guid.NewGuid().ToString(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new RoleAlreadyExistsResponse() };
         yield return new object[] { new RoleCannotBeUpdatedException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new RoleCannotBeUpdatedResponse() };
         yield return new object[] { new RoleNotFoundException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new RoleNotFoundResponse() };
         yield return new object[] { new StubNotFoundException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new StubNotFoundResponse() };
         yield return new object[] { new TeamNotFoundException(ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new TeamNotFoundResponse() };
         yield return new object[] { new UnknownIdentityIdException(), HttpStatusCode.Unauthorized, new UnknownIdentityIdResponse() };
         yield return new object[] { new UserRoleNotFoundException(), HttpStatusCode.InternalServerError, new ErrorResponse() };
      }
   }
}