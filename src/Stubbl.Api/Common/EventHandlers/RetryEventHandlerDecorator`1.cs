namespace Stubbl.Api.Common.EventHandlers
{
    using Events;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Newtonsoft.Json;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RetryEventHandlerDecorator<TEvent> : IEventHandler<TEvent>
       where TEvent : IEvent
    {
        private readonly IConfiguration _configuration;
        private readonly IEventHandler<TEvent> _decorated;
        private readonly IEventIdAccessor _eventIdAccessor;
        private readonly ILogger<RetryEventHandlerDecorator<TEvent>> _logger;

        public RetryEventHandlerDecorator(IConfiguration configuration, IEventHandler<TEvent> decorated,
            IEventIdAccessor eventIdAccessor, ILogger<RetryEventHandlerDecorator<TEvent>> logger)
        {
            _configuration = configuration;
            _decorated = decorated;
            _eventIdAccessor = eventIdAccessor;
            _logger = logger;
        }

        public async Task HandleAsync(TEvent @event, CancellationToken cancellationToken)
        {
            const int retryCount = 3;
            const int retryIntervalInMilliseconds = 500;

            var eventName = @event.GetType().FullName;

            for (var i = 0; i < retryCount; i++)
            {
                try
                {
                    Thread.Sleep(i * retryIntervalInMilliseconds);

                    await _decorated.HandleAsync(@event, cancellationToken);

                    return;
                }
                catch (Exception exception)
                {
                    var exceptionName = exception.GetType().FullName;

                    if (i == retryCount - 1)
                    {
                        _logger.LogError
                        (
                           _eventIdAccessor.EventId,
                           exception,
                           "Exception {ExceptionName} thrown with message {ExceptionMessage} when handling event {EventName}",
                           exceptionName,
                           exception.Message,
                           eventName
                        );
                    }
                }
            }

            try
            {
                var storageAccount = CloudStorageAccount.Parse(_configuration.GetValue<string>("Storage:ConnectionString"));
                var queueClient = storageAccount.CreateCloudQueueClient();
                var queue = queueClient.GetQueueReference($"{eventName.ToLower()}-poison");
                await queue.CreateIfNotExistsAsync();
                var message = new CloudQueueMessage(JsonConvert.SerializeObject(@event));
                await queue.AddMessageAsync(message);
            }
            catch (Exception exception)
            {
                var exceptionName = exception.GetType().FullName;

                _logger.LogError
                (
                   _eventIdAccessor.EventId,
                   exception,
                   "Exception {ExceptionName} thrown with message {ExceptionMessage} when storing poisoned event {EventName}",
                   exceptionName,
                   exception.Message,
                   eventName
                );
            }
        }
    }
}