//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;
//using Dafmis.Shared.PubSub;
//using Dafmis.Shared.PubSub.KafkaImpl;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using Dafmis.Shared.Extensions;
//using MediatR;
//using Dafmis.Shared.EventModels;

//namespace Dafmis.Shared.Net.WorkerService
//{
//    public class EvenHubProcessorService : BackgroundService
//    {
//        //private readonly ILogger<MessageProcessorService> _logger;
//        private readonly BoundedMessageChannel<BusMessage> _boundedMessageChannel;
//        private readonly IMediator _mediator;
//        private readonly List<BusHandler> _busDelegate;

//        public EvenHubProcessorService(BoundedMessageChannel<BusMessage> boundedMessageChannel, IMediator mediator)
//        {
//            // _logger = logger;
//            _boundedMessageChannel = boundedMessageChannel;
//            _mediator = mediator;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                try
//                {
//                    var message = await _boundedMessageChannel.ReadAsync(stoppingToken);

//                    if (message.Value == null)
//                        continue;

//                    var busMsg = message.Value;

//                    switch ((BusMessageTypes)busMsg.BusMessageType)
//                    {
//                        case BusMessageTypes.NEW_USER:
//                            await _mediator.Publish(new SetupUserEvent(busMsg.Data));
//                            break;
//                        case BusMessageTypes.EMAIL:
//                            await _mediator.Publish(new EmailEvent(busMsg.Data));
//                            break;
//                        case BusMessageTypes.NEW_MARKETER:
//                            await _mediator.Publish(new SetupMarketerEvent(busMsg.Data));
//                            break;
//                        default:
//                            break;
//                    }
//                }
//                catch (OperationCanceledException)
//                {
//                    // Log an swallow as the while loop will end gracefully when cancellation has been requested
//                    //_logger.OperationCancelledExceptionOccurred();
//                }
//                catch (Exception ex)
//                {
//                    // If errors occur, we will probably send this to a poison queue, allow the message 
//                    // to be deleted and continue processing other messages.
//                    //_logger.ExceptionOccurred(ex);
//                    // Note: Assumes no roll back is needed due to partial success for various processing tasks.
//                }
//            }
//        }

//        public override async Task StopAsync(CancellationToken cancellationToken)
//        {
//            await base.StopAsync(cancellationToken);
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Imanage.Shared.PubSub;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Imanage.Shared.Net.WorkerService
{
    public class EvenHubProcessorService : BackgroundService
    {
        //private readonly ILogger<MessageProcessorService> _logger;
        private readonly BoundedMessageChannel<BusMessage> _boundedMessageChannel;
        private readonly List<BusHandler> _busDelegate;

        public EvenHubProcessorService(BoundedMessageChannel<BusMessage> boundedMessageChannel,
            Func<List<BusHandler>> busDelegate)
        {
            // _logger = logger;
            _boundedMessageChannel = boundedMessageChannel;
            _busDelegate = busDelegate();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) {
                try {
                    var message = await _boundedMessageChannel.ReadAsync(stoppingToken);

                    if (message.Value == null)
                        continue;

                    var busMsg = message.Value;
                    foreach (var item in _busDelegate) {
                        item(busMsg);
                    }
                }
                catch (OperationCanceledException) {
                    // Log an swallow as the while loop will end gracefully when cancellation has been requested
                    //_logger.OperationCancelledExceptionOccurred();
                }
                catch (Exception e) {
                    // If errors occur, we will probably send this to a poison queue, allow the message 
                    // to be deleted and continue processing other messages.
                    //_logger.ExceptionOccurred(ex);
                    // Note: Assumes no roll back is needed due to partial success for various processing tasks.
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
