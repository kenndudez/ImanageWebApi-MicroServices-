using System;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Imanage.Shared.PubSub;

namespace Imanage.Shared.Net.WorkerService
{
    public class EventHubReaderService : BackgroundService
    {
        private readonly BoundedMessageChannel<BusMessage> _boundedMessageChannel;
        private readonly IConsumerClient<BusMessage> _consumer;

        public EventHubReaderService(BoundedMessageChannel<BusMessage> boundedMessageChannel,
            IConsumerClient<BusMessage> consumer)
        {
            _consumer = consumer;
            _boundedMessageChannel = boundedMessageChannel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_logger.LogDebug($"Event bus is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var message = _consumer.Poll(stoppingToken);
                    
                    if (message == null)
                    {
                        //Todo:log here
                        continue;
                    }
                    await _boundedMessageChannel.WriteMessageAsync(message, stoppingToken);
                    //await Task.Delay(TimeSpan.FromSeconds(8), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Log an swallow as the while loop will end gracefully when cancellation has been requested
                    //_logger.OperationCancelledExceptionOccurred();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
