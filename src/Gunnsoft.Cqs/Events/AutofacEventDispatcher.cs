namespace Gunnsoft.Cqs.Events
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;
   using System.Threading;
   using System.Threading.Tasks;
   using Autofac;
   using EventHandlers;

   public class AutofacEventDispatcher : IEventDispatcher
   {
      private readonly IComponentContext _componentContext;
      private static readonly Type s_eventHandlerGenericType;
      private static readonly Type s_eventHandlersGenericType;

      static AutofacEventDispatcher()
      {
         s_eventHandlersGenericType = typeof(IReadOnlyCollection<>);
         s_eventHandlerGenericType = typeof(IEventHandler<>);
      }

      public AutofacEventDispatcher(IComponentContext componentContext)
      {
         _componentContext = componentContext;
      }

      public async Task DispatchAsync(IEvent @event, CancellationToken cancellationToken)
      {
         var eventHandlerType = s_eventHandlerGenericType.MakeGenericType(@event.GetType());
         var handleMethod = eventHandlerType.GetTypeInfo().GetMethod("HandleAsync");
         var collectionType = s_eventHandlersGenericType.MakeGenericType(eventHandlerType);
         var eventHandlers = _componentContext.Resolve(collectionType) as dynamic;

         if ( eventHandlers == null )
         {
            return;
         }

         var exceptions = new List<Exception>();

         foreach (var eventHandler in eventHandlers)
         {
            try
            {
               await handleMethod.Invoke(eventHandler, new object[] { @event, cancellationToken });
            }
            catch (TargetInvocationException exception)
            {
               exceptions.Add(exception.InnerException ?? exception);
            }
         }

         if (exceptions.Any())
         {
            throw new AggregateException(exceptions);
         }
      }
   }
}