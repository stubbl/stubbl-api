namespace Stubbl.Api.Tests.Integration.Filters.Version1
{
   using System.Collections;
   using System.Net.Http;
   using MongoDB.Bson;

   public class MalformedJsonTextFixtureData : IEnumerable
   {
      public IEnumerator GetEnumerator()
      {
         yield return new object[] { HttpMethod.Post, "/member/update" };

         yield return new object[] { HttpMethod.Post, "/teams/create" };
         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/update" };

         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/invitations/create" };

         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/roles/create" };
         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/roles/{ObjectId.GenerateNewId()}/update" };

         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/stubs/create" };
         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/stubs/{ObjectId.GenerateNewId()}/clone" };
         yield return new object[] { HttpMethod.Post, $"/teams/{ObjectId.GenerateNewId()}/stubs/{ObjectId.GenerateNewId()}/update" };
      }
   }
}