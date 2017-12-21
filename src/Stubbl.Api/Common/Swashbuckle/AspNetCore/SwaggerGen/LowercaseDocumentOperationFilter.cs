namespace Swashbuckle.AspNetCore.SwaggerGen
{
   using Swashbuckle.AspNetCore.Swagger;
   using System.Collections.Generic;

   public class LowercaseDocumentOperationFilter : IDocumentFilter
   {
      public void Apply(SwaggerDocument swaggerDocument, DocumentFilterContext context)
      {
         var paths = swaggerDocument.Paths;

         var newPaths = new Dictionary<string, PathItem>();
         var removeKeys = new List<string>();

         foreach (var path in paths)
         {
            string newKey = path.Key.ToLower();

            if (newKey != path.Key)
            {
               removeKeys.Add(path.Key);
               newPaths.Add(newKey, path.Value);
            }
         }

         foreach (var path in newPaths)
         {
            swaggerDocument.Paths.Add(path.Key, path.Value);
         }

         foreach (string key in removeKeys)
         {
            swaggerDocument.Paths.Remove(key);
         }
      }
   }
}
