﻿namespace Microsoft.AspNetCore.Routing
{
    using Http;
    using MongoDB.Bson;

    public class ObjectIdRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (!values.TryGetValue(routeKey, out var value) || value == null) return false;

            return ObjectId.TryParse(value.ToString(), out var _);
        }
    }
}