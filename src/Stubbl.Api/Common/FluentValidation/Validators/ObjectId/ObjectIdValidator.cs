namespace FluentValidation.Validators.ObjectId
{
   using MongoDB.Bson;

   public class ObjectIdValidator : PropertyValidator
   {
      public ObjectIdValidator()
         : base("'{PropertyName}' must be a valid ID.")
      {
      }

      protected override bool IsValid(PropertyValidatorContext context)
      {
         var value = context.PropertyValue;

         return value != null && ObjectId.TryParse(value.ToString(), out ObjectId _);
      }
   }
}