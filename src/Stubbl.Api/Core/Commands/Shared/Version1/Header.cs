﻿namespace Stubbl.Api.Core.Commands.Shared.Version1
{
   public class Header
   {
      public Header(string key, string value)
      {
         Key = key;
         Value = value;
      }

      public string Key { get; }
      public string Value { get; }
   }
}