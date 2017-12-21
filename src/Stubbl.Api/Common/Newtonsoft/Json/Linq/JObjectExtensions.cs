namespace Newtonsoft.Json.Linq
{
   using System.Collections.Generic;
   using System.Linq;

   public static class JObjectExtensions
   {
      public static void Add(this JObject extended, IReadOnlyCollection<string> pathParts, JToken value)
      {
         var firstPathPath = pathParts.First();

         if (pathParts.Count == 1)
         {
            extended.Add(firstPathPath, value);

            return;
         }

         var jToken = extended.SelectToken(firstPathPath);
         JObject childJObject;

         if (jToken == null)
         {
            childJObject = new JObject
            {
               { pathParts.Skip(1).ToList(), value }
            };
            extended.Add(firstPathPath, childJObject);
         }
         else
         {
            childJObject = jToken as JObject;

            if (childJObject == null)
            {
               return;
            }

            childJObject.Add(pathParts.Skip(1).ToList(), value);
         }
      }
   }
}