namespace Stubbl.Api.Models.TeamNotFound.Version1
{
   using Error.Version1;

   public class TeamNotFoundResponse : ErrorResponse
   {
      public TeamNotFoundResponse()
         : base("TeamNotFound", "The team cannot be found.")
      {
      }
   }
}