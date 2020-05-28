using System;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.DotNet.PlatformAbstractions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Imanage.Shared.PubSub.KafkaImpl
{
    public class ProducerClient<T>: IProducerClient<T> where T: new()
    {
        private readonly ProducerConfig producerConfig;
        private readonly IProducer<Null, T> producer;
        private readonly IConfiguration _globalconf;

        public ProducerClient(IHostingEnvironment env, IConfiguration globalconf)
        {
            _globalconf = globalconf;

            string sslCaLocation = $@"{ApplicationEnvironment.ApplicationBasePath}{globalconf.GetValue<string>("Kafka:Cert")}";

            producerConfig = new ProducerConfig
            {
                BootstrapServers = globalconf.GetValue<string>("Kafka:BootstrapServers"),
                ApiVersionFallbackMs = 0,
                //Debug = "security,broker,protocol"
            };
            if (globalconf.GetValue<bool>("Kafka:UseSSL")) {
                producerConfig.SecurityProtocol = SecurityProtocol.SaslSsl;
                producerConfig.SaslMechanism = SaslMechanism.Plain;
                producerConfig.SslCaLocation = sslCaLocation;
            }

            if (globalconf.GetValue<bool>("Kafka:RequireLogin")) {
                producerConfig.SaslUsername = globalconf.GetValue<string>("Kafka:Username");
                producerConfig.SaslPassword = globalconf.GetValue<string>("Kafka:Password");
            }

            //if (env.IsDevelopment())
            //{
            //    producerConfig = new ProducerConfig
            //    {
            //        BootstrapServers = globalconf.GetValue<string>("Kafka:BootstrapServers"),
            //        SecurityProtocol = SecurityProtocol.SaslSsl,
            //        SaslMechanism = SaslMechanism.Plain,
            //        SaslUsername = globalconf.GetValue<string>("Kafka:Username"),
            //        SaslPassword = globalconf.GetValue<string>("Kafka:Password"),
            //        SslCaLocation = sslCaLocation,
            //        ApiVersionFallbackMs = 0,
            //        Debug = "security,broker,protocol"
            //    };
            //    producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            //}
            //else
            //{
            //    producerConfig = new ProducerConfig
            //    {
            //        BootstrapServers = globalconf.GetValue<string>("Kafka:BootstrapServers"),
            //        SecurityProtocol = SecurityProtocol.SaslSsl,
            //        SaslMechanism = SaslMechanism.Plain,
            //        SaslUsername = globalconf.GetValue<string>("Kafka:Username"),
            //        SaslPassword = globalconf.GetValue<string>("Kafka:Password"),
            //        SslCaLocation = sslCaLocation,
            //        ApiVersionFallbackMs = 0,
            //    };
            //}

            var producerBuilder = new ProducerBuilder<Null, T>(producerConfig);
            producerBuilder.SetValueSerializer(new KafkaByteSerializer<T>());
            producer = producerBuilder.Build();
        }

        public async Task<DeliveryResult<Null, T>> Produce(string topic, T message)
        {
            //Append Environment to Topic
            var envName = _globalconf.GetSection("Kafka").GetValue<string>("Environment").ToString();
            topic = $"{envName}_{topic}";
            var msg = new Message<Null, T>();
            msg.Value = message;
            return await producer.ProduceAsync(topic, msg);
        }
    }
}
