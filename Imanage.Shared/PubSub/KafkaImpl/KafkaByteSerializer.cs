using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Imanage.Shared.Extensions;

namespace Imanage.Shared.PubSub.KafkaImpl
{
    public class KafkaByteSerializer<T> : ISerializer<T> where T: new()
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
           return data.SerializeToBytes();
        }
    }
}
