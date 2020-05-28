using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Imanage.Shared.PubSub
{
    public interface IProducerClient<T>
    {
        Task<DeliveryResult<Null, T>> Produce(string topic, T message);

    }
}
