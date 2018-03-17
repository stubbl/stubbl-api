using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpResponseExtensions
    {
        public static async Task WriteJsonAsync(this HttpResponse extended, HttpStatusCode statusCode, object value,
            JsonSerializerSettings jsonSerializerSettings)
        {
            extended.ContentType = "application/json";
            extended.StatusCode = (int) statusCode;

            var text = JsonConvert.SerializeObject(value, Formatting.Indented, jsonSerializerSettings);

            await extended.WriteAsync(text, Encoding.UTF8);
        }
    }
}