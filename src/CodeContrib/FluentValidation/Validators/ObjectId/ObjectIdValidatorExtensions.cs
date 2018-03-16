namespace FluentValidation.Validators.ObjectId
{
   public static class ObjectIdValidatorExtensions
   {
      public static IRuleBuilderOptions<T, string> ObjectId<T>(this IRuleBuilder<T, string> extended)
      {
         return extended.SetValidator(new ObjectIdValidator());
      }
   }
}