using FluentValidation.Validators;

namespace Gunnsoft.Api.FluentValidation.Validators
{
    public class ObjectIdValidator : PropertyValidator
    {
        public ObjectIdValidator()
            : base("'{PropertyName}' must be a valid ObjectId.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue;

            return value != null && MongoDB.Bson.ObjectId.TryParse(value.ToString(), out MongoDB.Bson.ObjectId _);
        }
    }
}