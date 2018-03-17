using Newtonsoft.Json.Linq;

namespace Stubbl.Api.Commands.Shared.Version1
{
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