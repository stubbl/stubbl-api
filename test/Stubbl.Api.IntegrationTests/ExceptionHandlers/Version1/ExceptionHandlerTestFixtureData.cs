using System;
using System.Collections;
using System.Net;
using Gunnsoft.Api.Exceptions.AuthenticatedUserNotFound.Version1;
using Gunnsoft.Api.Exceptions.UnknownSub.Version1;
using Gunnsoft.Api.Models.AuthenticatedUserNotFound.Version1;
using Gunnsoft.Api.Models.Error.Version1;
using Gunnsoft.Api.Models.UnknownSub.Version1;
using MongoDB.Bson;
using Stubbl.Api.Exceptions.AdministratorRoleNotFound.Version1;
using Stubbl.Api.Exceptions.InvitationAlreadyUsed.Version1;
using Stubbl.Api.Exceptions.InvitationNotFound.Version1;
using Stubbl.Api.Exceptions.LogNotFound.Version1;
using Stubbl.Api.Exceptions.MemberAlreadyAddedToTeam.Version1;
using Stubbl.Api.Exceptions.MemberAlreadyInvitedToTeam.Version1;
using Stubbl.Api.Exceptions.MemberCannotBeRemovedFromTeam.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageInvitations.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageMembers.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageRoles.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageStubs.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageTeams.Version1;
using Stubbl.Api.Exceptions.MemberNotFound.Version1;
using Stubbl.Api.Exceptions.RoleAlreadyExists.Version1;
using Stubbl.Api.Exceptions.RoleCannotBeUpdated.Version1;
using Stubbl.Api.Exceptions.RoleNotFound.Version1;
using Stubbl.Api.Exceptions.StubNotFound.Version1;
using Stubbl.Api.Exceptions.TeamNotFound.Version1;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using Stubbl.Api.Exceptions.UserNotInvitedToTeam.Version1;
using Stubbl.Api.Exceptions.UserRoleNotFound.Version1;
using Stubbl.Api.Models.InvitationAlreadyUsed.Version1;
using Stubbl.Api.Models.InvitationNotFound.Version1;
using Stubbl.Api.Models.LogNotFound.Version1;
using Stubbl.Api.Models.MemberAlreadyAddedToTeam.Version1;
using Stubbl.Api.Models.MemberAlreadyInvitedToTeam.Version1;
using Stubbl.Api.Models.MemberCannotBeRemovedFromTeam.Version1;
using Stubbl.Api.Models.MemberCannotManageInvitations.Version1;
using Stubbl.Api.Models.MemberCannotManageMembers.Version1;
using Stubbl.Api.Models.MemberCannotManageRoles.Version1;
using Stubbl.Api.Models.MemberCannotManageStubs.Version1;
using Stubbl.Api.Models.MemberCannotManageTeams.Version1;
using Stubbl.Api.Models.MemberNotAddedToTeam.Version1;
using Stubbl.Api.Models.MemberNotFound.Version1;
using Stubbl.Api.Models.MemberNotInvitedToTeam.Version1;
using Stubbl.Api.Models.RoleAlreadyExists.Version1;
using Stubbl.Api.Models.RoleCannotBeUpdated.Version1;
using Stubbl.Api.Models.RoleNotFound.Version1;
using Stubbl.Api.Models.StubNotFound.Version1;
using Stubbl.Api.Models.TeamNotFound.Version1;

namespace Stubbl.Api.IntegrationTests.ExceptionHandlers.Version1
{
    public class ExceptionHandlerTestFixtureData : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[]
                {new AdministratorRoleNotFoundException(), HttpStatusCode.InternalServerError, new ErrorResponse()};
            yield return new object[]
            {
                new AuthenticatedUserNotFoundException(null), HttpStatusCode.Unauthorized,
                new AuthenticatedUserNotFoundResponse()
            };
            yield return new object[] {new Exception(), HttpStatusCode.InternalServerError, new ErrorResponse()};
            yield return new object[]
            {
                new InvitationAlreadyUsedException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Conflict, new InvitationAlreadyUsedResponse()
            };
            yield return new object[]
            {
                new InvitationNotFoundException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Conflict, new InvitationNotFoundResponse()
            };
            yield return new object[]
            {
                new LogNotFoundException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict,
                new LogNotFoundResponse()
            };
            yield return new object[]
            {
                new MemberAlreadyAddedToTeamException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Conflict, new MemberAlreadyAddedToTeamResponse()
            };
            yield return new object[]
            {
                new MemberAlreadyInvitedToTeamException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Conflict, new MemberAlreadyInvitedToTeamResponse()
            };
            yield return new object[]
            {
                new MemberCannotBeRemovedFromTeamException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Conflict, new MemberCannotBeRemovedFromTeamResponse()
            };
            yield return new object[]
            {
                new MemberCannotManageInvitationsException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Forbidden, new MemberCannotManageInvitationsResponse()
            };
            yield return new object[]
            {
                new MemberCannotManageMembersException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Forbidden, new MemberCannotManageMembersResponse()
            };
            yield return new object[]
            {
                new MemberCannotManageRolesException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Forbidden, new MemberCannotManageRolesResponse()
            };
            yield return new object[]
            {
                new MemberCannotManageStubsException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Forbidden, new MemberCannotManageStubsResponse()
            };
            yield return new object[]
            {
                new MemberCannotManageTeamsException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Forbidden, new MemberCannotManageTeamsResponse()
            };
            yield return new object[]
            {
                new UserNotAddedToTeamException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Forbidden, new MemberNotAddedToTeamResponse()
            };
            yield return new object[]
            {
                new MemberNotFoundException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Conflict, new MemberNotFoundResponse()
            };
            yield return new object[]
            {
                new UserNotInvitedToTeamException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Conflict, new MemberNotInvitedToTeamResponse()
            };
            yield return new object[]
            {
                new RoleAlreadyExistsException(Guid.NewGuid().ToString(), ObjectId.GenerateNewId()),
                HttpStatusCode.Conflict, new RoleAlreadyExistsResponse()
            };
            yield return new object[]
            {
                new RoleCannotBeUpdatedException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()),
                HttpStatusCode.Conflict, new RoleCannotBeUpdatedResponse()
            };
            yield return new object[]
            {
                new RoleNotFoundException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict,
                new RoleNotFoundResponse()
            };
            yield return new object[]
            {
                new StubNotFoundException(ObjectId.GenerateNewId(), ObjectId.GenerateNewId()), HttpStatusCode.Conflict,
                new StubNotFoundResponse()
            };
            yield return new object[]
            {
                new TeamNotFoundException(ObjectId.GenerateNewId()), HttpStatusCode.Conflict, new TeamNotFoundResponse()
            };
            yield return new object[]
                {new UnknownSubException(), HttpStatusCode.Unauthorized, new UnknownSubResponse()};
            yield return new object[]
                {new UserRoleNotFoundException(), HttpStatusCode.InternalServerError, new ErrorResponse()};
        }
    }
}