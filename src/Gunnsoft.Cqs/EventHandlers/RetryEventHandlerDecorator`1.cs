using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Gunnsoft.Cqs.EventHandlers
{
    public class RetryEventHandlerDecorator<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        private readonly CqsSettings _cqsSettings;
        private readonly IEventHandler<TEvent> _decorated;
        private readonly ILogger<RetryEventHandlerDecorator<TEvent>> _logger;

        public RetryEventHandlerDecorator(CqsSettings cqsSettings, IEventHandler<TEvent> decorated,
            ILogger<RetryEventHandlerDecorator<TEvent>> logger)
        {
            _cqsSettings = cqsSettings;
            _decorated = decorated;
            _logger = logger;
        }

        public async Task HandleAsync(TEvent @event, CancellationToken cancellationToken)
        {
            const int retryCount = 3;
            const int retryIntervalInMilliseconds = 500;

            var eventName = @event.GetType().FullName;
            var exceptions = new List<Exception>();

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
                    exceptions.Add(exception);

                    var exceptionName = exception.GetType().FullName;

                    if (i == retryCount - 1)
                    {
                        _logger.LogWarning
                        (
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
                var storageAccount = CloudStorageAccount.Parse(_cqsSettings.StorageConnectionString);
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
                    exception,
                    "Exception {ExceptionName} thrown with message {ExceptionMessage} when storing poisoned event {EventName}",
                    exceptionName,
                    exception.Message,
                    eventName
                );
            }

            throw new AggregateException(exceptions);
        }
    }
}