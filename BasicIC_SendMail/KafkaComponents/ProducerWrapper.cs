using BasicIC_SendEmail.App_Start;
using BasicIC_SendEmail.Config;
using Common;
using Common.Commons;
using Common.Interfaces;
using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SocialConnection.KafkaComponents
{
    public static class ProducerFactory
    {
        private static readonly ProducerConfig _producerConfig = new ProducerConfig
        {
            BootstrapServers = ConfigManager.StaticGet(Constants.CONF_KAFKA_BOOSTRAP_SERVER),
            ClientId = Dns.GetHostName()
        };
        public static readonly IProducer<string, string> _producer;

        static ProducerFactory()
        {
            _producer = new ProducerBuilder<string, string>(_producerConfig).Build();
        }
    }

    public class ProducerWrapper<T> where T : class
    {
        private readonly ILogger _logger = NinjectWebCommon.CreateInstanceDJ<ILogger>();

        public ProducerWrapper()
        {
        }

        public async Task<ResponseMessage<T>> CreateMess(string topic, T mess, string tenantId = null)
        {
            try
            {
                var dr = await ProducerFactory._producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = JsonConvert.SerializeObject(new ResponseMessage<T>(topic, mess, tenantId))
                });

                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");

                return new ResponseMessage<T>(topic, mess, tenantId);

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Join("\n", ex.Message));
                return new ResponseMessage<T>(topic, ex);
            }
        }
    }

    //public class ProducerWrapper<T> where T : class
    //{
    //    private readonly ProducerConfig _producerConfig;
    //    private readonly ILogger _logger = NinjectWebCommon.CreateInstanceDJ<ILogger>();

    //    public ProducerWrapper()
    //    {
    //        _producerConfig = new ProducerConfig
    //        {
    //            BootstrapServers = ConfigManager.StaticGet(Constants.CONF_KAFKA_BOOSTRAP_SERVER),
    //            ClientId = Dns.GetHostName()
    //        };
    //    }

    //    public async Task<ResponseMessage<T>> CreateMess(string topic, T mess, string tenantId = null)
    //    {
    //        try
    //        {
    //            string token = SessionStore.Get(Constants.KEY_SESSION_TOKEN);
    //            string email = SessionStore.Get(Constants.KEY_SESSION_EMAIL);

    //            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
    //            {
    //                var dr = await producer.ProduceAsync(topic, new Message<string, string>
    //                {
    //                    Key = Guid.NewGuid().ToString(),
    //                    Value = JsonConvert.SerializeObject(new ResponseMessage<T>(topic, mess, token, tenantId, email))
    //                });

    //                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
    //                return new ResponseMessage<T>(topic, mess, token);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(string.Join("\n", ex.Message));
    //            return new ResponseMessage<T>(topic, ex);
    //        }
    //    }
    //}
}