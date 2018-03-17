using System;
using FluentValidation.Validators;
using Stubbl.Api.Commands.Shared.Version1;

namespace Stubbl.Api.Models.Shared.Version1
{
    public class PermissionValidator : PropertyValidator
    {
        public PermissionValidator()
            : base("'{PropertyName}' must be a valid permission.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue as string;

            if (value == null)
            {
                return false;
            }

            return Enum.TryParse(value, out Permission _);
        }
    }
}