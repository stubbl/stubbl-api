using System;

namespace Stubbl.Api.Exceptions.EmailAdressAlreadyUsed.Version1
{
    public class EmailAddressAlreadyTakenException : Exception
    {
        public EmailAddressAlreadyTakenException(string emailAddress)
            : base($"Email address has already been taken. EmailAddress='{emailAddress}'")
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }
    }
}