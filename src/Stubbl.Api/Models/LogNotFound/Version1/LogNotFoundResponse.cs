using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.LogNotFound.Version1
{
    public class LogNotFoundResponse : ErrorResponse
    {
        public LogNotFoundResponse()
            : base("LogNotFound", "The log cannot be found.")
        {
        }
    }
}