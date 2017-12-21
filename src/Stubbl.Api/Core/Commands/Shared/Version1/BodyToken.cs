namespace Stubbl.Api.Core.Commands.Shared.Version1
{
   using Newtonsoft.Json.Linq;

   public class BodyToken
   {
      public BodyToken(string path, JTokenType type, string value)
      {
         Path = path;
         Type = type;
         Value = value;
      }

      public string Path { get; }
      public JTokenType Type { get; }
      public string Value { get; }
   }
}