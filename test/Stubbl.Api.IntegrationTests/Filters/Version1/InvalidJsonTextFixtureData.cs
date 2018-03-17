using System.Collections;
using System.Net.Http;
using MongoDB.Bson;
using Stubbl.Api.Models.CloneTeamStub.Version1;
using Stubbl.Api.Models.CreateTeam.Version1;
using Stubbl.Api.Models.CreateTeamInvitation.Version1;
using Stubbl.Api.Models.CreateTeamRole.Version1;
using Stubbl.Api.Models.CreateTeamStub.Version1;
using Stubbl.Api.Models.UpdateAuthenticatedUser.Version1;
using Stubbl.Api.Models.UpdateTeam.Version1;
using Stubbl.Api.Models.UpdateTeamRole.Version1;
using Stubbl.Api.Models.UpdateTeamStub.Version1;

namespace Stubbl.Api.IntegrationTests.Filters.Version1
{
    public class InvalidJsonTextFixtureData : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[]
            {
                HttpMethod.Post, "/user/update", new UpdateAuthenticatedUserRequest(),
                typeof(UpdateAuthenticatedUserRequestValidator)
            };

            yield return new object[]
                {HttpMethod.Post, "/teams/create", new CreateTeamRequest(), typeof(CreateTeamRequestValidator)};
            yield return new object[]
            {
                HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/update", new UpdateTeamRequest(),
                typeof(UpdateTeamRequestValidator)
            };

            yield return new object[]
            {
                HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/invitations/create",
                new CreateTeamInvitationRequest(), typeof(CreateTeamInvitationRequestValidator)
            };

            yield return new object[]
            {
                HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/roles/create", new CreateTeamRoleRequest(),
                typeof(CreateTeamRoleRequestValidator)
            };
            yield return new object[]
            {
                HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/roles/{ObjectId.GenerateNewId()}/update",
                new UpdateTeamRoleRequest(), typeof(UpdateTeamRoleRequestValidator)
            };

            yield return new object[]
            {
                HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/stubs/create", new CreateTeamStubRequest(),
                typeof(CreateTeamStubRequestValidator)
            };
            yield return new object[]
            {
                HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/stubs/{ObjectId.GenerateNewId()}/clone",
                new CloneTeamStubRequest(), typeof(CloneTeamStubRequestValidator)
            };
            yield return new object[]
            {
                HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/stubs/{ObjectId.GenerateNewId()}/update",
                new UpdateTeamStubRequest(), typeof(UpdateTeamStubRequestValidator)
            };
        }
    }
}