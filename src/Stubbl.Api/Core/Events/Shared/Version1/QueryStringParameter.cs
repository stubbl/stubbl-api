﻿namespace Stubbl.Api.Core.Events.Shared.Version1
{
   public class QueryStringParameter
   {
      public QueryStringParameter(string key, string value)
      {
         Key = key;
         Value = value;
      }

      public string Key { get; }
      public string Value { get; }
   }
}