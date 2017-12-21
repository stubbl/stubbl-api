namespace Stubbl.Api.Tests.Integration.Filters.Version1
{
   using System.Collections;
   using System.Net.Http;
   using Models.CloneTeamStub.Version1;
   using Models.CreateTeam.Version1;
   using Models.CreateTeamInvitation.Version1;
   using Models.CreateTeamRole.Version1;
   using Models.CreateTeamStub.Version1;
   using Models.UpdateAuthenticatedMember.Version1;
   using Models.UpdateTeam.Version1;
   using Models.UpdateTeamRole.Version1;
   using Models.UpdateTeamStub.Version1;
   using MongoDB.Bson;

   public class InvalidJsonTextFixtureData : IEnumerable
   {
      public IEnumerator GetEnumerator()
      {
         yield return new object[] { HttpMethod.Post, "/member/update", new UpdateAuthenticatedMemberRequest(), typeof(UpdateAuthenticatedMemberRequestValidator) };

         yield return new object[] { HttpMethod.Post, "/teams/create", new CreateTeamRequest(), typeof(CreateTeamRequestValidator) };
         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/update", new UpdateTeamRequest(), typeof(UpdateTeamRequestValidator) };

         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/invitations/create", new CreateTeamInvitationRequest(), typeof(CreateTeamInvitationRequestValidator) };

         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/roles/create", new CreateTeamRoleRequest(), typeof(CreateTeamRoleRequestValidator) };
         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/roles/{ObjectId.GenerateNewId()}/update", new UpdateTeamRoleRequest(), typeof(UpdateTeamRoleRequestValidator) };

         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/stubs/create", new CreateTeamStubRequest(), typeof(CreateTeamStubRequestValidator) };
         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/stubs/{ObjectId.GenerateNewId()}/clone", new CloneTeamStubRequest(), typeof(CloneTeamStubRequestValidator) };
         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/stubs/{ObjectId.GenerateNewId()}/update", new UpdateTeamStubRequest(), typeof(UpdateTeamStubRequestValidator) };
      }
   }
}