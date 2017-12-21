namespace Stubbl.Api.Models.Shared.Version1
{
   using System;
   using Core.Commands.Shared.Version1;
   using FluentValidation.Validators;

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