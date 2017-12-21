namespace Stubbl.Api.Core.Data.Collections.Stubs
{
   using Newtonsoft.Json.Linq;

   public class BodyToken
   {
      public string Path { get; set; }
      public JTokenType Type { get; set; }
      public string Value { get; set; }
   }
}