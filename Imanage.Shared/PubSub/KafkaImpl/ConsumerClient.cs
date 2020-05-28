using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Confluent.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;

namespace Imanage.Shared.PubSub.KafkaImpl
{
    public class ConsumerClient<T> : IConsumerClient<T> where T : new()
    {
        private readonly ConsumerConfig consumerConfig;
        private Action<string> messageReceived;
        private readonly IConsumer<Ignore, T> consumer;

        private Action<string> GetMessageReceived()
        {
            return messageReceived;
        }

        private void SetMessageReceived(Action<string> value)
        {
            messageReceived = value;
        }

        public event Action<string> OnMessageRecieved
        {
            add
            {
                SetMessageReceived(GetMessageReceived() + value);
            }
            remove
            {
                SetMessageReceived(GetMessageReceived() - value);
            }
        }

        public ConsumerClient(IHostingEnvironment env, IConfiguration globalconf)
        {
            string sslCaLocation = $@"{ApplicationEnvironment.ApplicationBasePath}{globalconf.GetValue<string>("Kafka:Cert")}";

            consumerConfig = new ConsumerConfig
            {
                //Append Environment to GroupId
                GroupId = $"{globalconf.GetValue<string>("Kafka:Environment")}_{ globalconf.GetValue<string>("Kafka:GroupId")}",
                BootstrapServers = globalconf.GetValue<string>("Kafka:BootstrapServers"),
                //Debug = "security,broker,protocol",
                ApiVersionRequestTimeoutMs = 60000,
                AutoOffsetReset = AutoOffsetReset.Latest,
            };

            if (globalconf.GetValue<bool>("Kafka:UseSSL")) {
                consumerConfig.SecurityProtocol = SecurityProtocol.SaslSsl;
                consumerConfig.SaslMechanism = SaslMechanism.Plain;
                consumerConfig.SslCaLocation = sslCaLocation;
            }

            if (globalconf.GetValue<bool>("Kafka:RequireLogin")) {
                consumerConfig.SaslUsername = "$ConnectionString";
                consumerConfig.SaslPassword = globalconf.GetValue<string>("Kafka:Password");
            }

            var topics = globalconf.GetSection("Kafka").GetValue<string>("Topics").ToString().Split(",");
            var envName = globalconf.GetSection("Kafka").GetValue<string>("Environment").ToString();
            //Append Env to Topics
            topics = topics.Select(x => $"{envName}_{x}").ToArray();

            var consumerBuilder = new ConsumerBuilder<Ignore, T>(consumerConfig);
            consumerBuilder.SetValueDeserializer(new KafkaByteDeserializer<T>());
            consumer = consumerBuilder.Build();
            consumer.Subscribe(topics);
        }

        public void Subscribe(List<string> topics) => consumer.Subscribe(topics);

        public ConsumeResult<Ignore, T> Poll(CancellationToken cancellationToken = default(CancellationToken))
        {
            try {
                return consumer.Consume(cancellationToken);
            }
            catch (ConsumeException) {
            }

            return null;

        }
    }
}
