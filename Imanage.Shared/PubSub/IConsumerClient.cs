using System;
using System.Collections.Generic;
using Confluent.Kafka;
using System.Threading;

namespace Imanage.Shared.PubSub
{
    public interface IConsumerClient<T>
    {
        ConsumeResult<Ignore, T> Poll(CancellationToken cancellationToken);
        void Subscribe(List<string> topics);
        event Action<string> OnMessageRecieved;
    }
}
