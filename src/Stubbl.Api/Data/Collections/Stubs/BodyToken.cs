using Newtonsoft.Json.Linq;

namespace Stubbl.Api.Data.Collections.Stubs
{
    public class BodyToken
    {
        public string Path { get; set; }
        public JTokenType Type { get; set; }
        public string Value { get; set; }
    }
}