using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.EmailAddressAlreadyTaken.Version1
{
    public class EmailAddressAlreadyTakenResponse : ErrorResponse
    {
        public EmailAddressAlreadyTakenResponse()
            : base("EmailAddressAlreadyTaken", "The email address has already been taken.")
        {
        }
    }
}