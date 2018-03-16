namespace CodeContrib.CommandHandlers
{
   using System;
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Commands;
   using Events;

   public class RetryCommandHandlerDecorator<TCommand, TEvent> : ICommandHandler<TCommand, TEvent>
      where TCommand : ICommand<TEvent>
      where TEvent : IEvent
   {
      private readonly ICommandHandler<TCommand, TEvent> _decorated;

      public RetryCommandHandlerDecorator(ICommandHandler<TCommand, TEvent> decorated)
      {
         _decorated = decorated;
      }

      public async Task<TEvent> HandleAsync(TCommand command, CancellationToken cancellationToken)
      {
         const int retryCount = 3;
         const int retryIntervalInMilliseconds = 500;

         var exceptions = new List<Exception>();

         for (var i = 0; i < retryCount; i++)
         {
            try
            {
               Thread.Sleep(i * retryIntervalInMilliseconds);

               return await _decorated.HandleAsync(command, cancellationToken);
            }
            catch (Exception exception)
            {
               exceptions.Add(exception);
            }
         }

         throw new AggregateException(exceptions);
      }
   }
}