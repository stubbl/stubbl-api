using FluentValidation;

namespace Gunnsoft.Api.FluentValidation.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> ObjectId<T>(this IRuleBuilder<T, string> extended)
        {
            return extended.SetValidator(new ObjectIdValidator());
        }
    }
}