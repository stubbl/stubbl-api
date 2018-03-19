using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Gunnsoft.Cqs.Commands
{
    public class RetryCommandHandlerDecorator<TCommand, TEvent> : ICommandHandler<TCommand, TEvent>
        where TCommand : ICommand<TEvent>
        where TEvent : IEvent
    {
        private readonly CqsSettings _cqsSettings;
        private readonly ICommandHandler<TCommand, TEvent> _decorated;
        private readonly ILogger<RetryEventHandlerDecorator<TEvent>> _logger;

        public RetryCommandHandlerDecorator(CqsSettings cqsSettings, ICommandHandler<TCommand, TEvent> decorated,
            ILogger<RetryEventHandlerDecorator<TEvent>> logger)
        {
            _cqsSettings = cqsSettings;
            _decorated = decorated;
            _logger = logger;
        }

        public async Task<TEvent> HandleAsync(TCommand command, CancellationToken cancellationToken)
        {
            const int retryCount = 3;
            const int retryIntervalInMilliseconds = 500;

            var commandName = command.GetType().FullName;
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

                    var exceptionName = exception.GetType().FullName;

                    if (i == retryCount - 1)
                    {
                        _logger.LogWarning
                        (
                            exception,
                            "Exception {ExceptionName} thrown with message {ExceptionMessage} when handling command {CommandName}",
                            exceptionName,
                            exception.Message,
                            commandName
                        );
                    }
                }
            }

            try
            {
                var storageAccount = CloudStorageAccount.Parse(_cqsSettings.StorageConnectionString);
                var queueClient = storageAccount.CreateCloudQueueClient();
                var queue = queueClient.GetQueueReference($"{commandName.ToLower()}-poison");
                await queue.CreateIfNotExistsAsync();
                var message = new CloudQueueMessage(JsonConvert.SerializeObject(command));
                await queue.AddMessageAsync(message);
            }
            catch (Exception exception)
            {
                var exceptionName = exception.GetType().FullName;

                _logger.LogError
                (
                    exception,
                    "Exception {ExceptionName} thrown with message {ExceptionMessage} when storing poisoned command {CommandName}",
                    exceptionName,
                    exception.Message,
                    commandName
                );
            }

            throw new AggregateException(exceptions);
        }
    }
}