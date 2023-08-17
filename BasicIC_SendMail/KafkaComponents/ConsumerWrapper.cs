using BasicIC_SendEmail.App_Start;
using BasicIC_SendEmail.Config;
using BasicIC_SendEmail.Models.Main;
using Common;
using Common.Interfaces;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using SocialConnection.KafkaComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BasicIC_SendEmail.KafkaComponents
{
    public class ConsumerWrapper
    {
        private readonly ConsumerConfig _consumerConfig;
        private static ILogger _logger = NinjectWebCommon.CreateInstanceDJ<ILogger>();
        private static IConsumer<string, string> consumer;
        private readonly int MAX_DEGREE_PARALLELISM = int.Parse(ConfigManager.StaticGet(Constants.CONF_MAX_DEGREE_PARALLELISM));

        public ConsumerWrapper()
        {
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = ConfigManager.StaticGet(Constants.CONF_KAFKA_BOOSTRAP_SERVER),
                GroupId = ConfigManager.StaticGet(Constants.CONF_KAFKA_GROUP_ID),
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
        }

        public async Task StartGetMess(string[] topics)
        {
            try
            {
                // Init topic before subscribe if topic not exist
                using (var adminClient = new AdminClientBuilder(_consumerConfig).Build())
                {
                    Metadata metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(30));
                    List<string> topicsApp = metadata.Topics.Select(a => a.Topic).ToList();
                    List<TopicSpecification> topicsNotExist = topics.Where(topic => !topicsApp.Any(item => item == topic))
                        .Select(item => new TopicSpecification { Name = item })
                        .ToList();
                    if (topicsNotExist != null && topicsNotExist.Count > 0) await adminClient.CreateTopicsAsync(topicsNotExist);
                }

                using (consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build())
                {
                    consumer.Subscribe(topics);

                    CancellationTokenSource cts = new CancellationTokenSource();
                    Console.CancelKeyPress += (_, e) =>
                    {
                        e.Cancel = true; // prevent the process from terminating.
                        cts.Cancel();
                    };
                    ActionBlock<ConsumerResultData> _actionBlock = new ActionBlock<ConsumerResultData>(async (p) =>
                    {
                        HandleProducer handleProducer = new HandleProducer();
                        await handleProducer.Register(p);
                    },
                    new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = MAX_DEGREE_PARALLELISM }
                    );
                    try
                    {
                        while (true)
                        {
                            try
                            {

                                if (_actionBlock.InputCount <= 0)
                                {
                                    var cr = consumer.Consume(cts.Token);
                                    ConsumerResultData data = new ConsumerResultData(cr, consumer);
                                    _actionBlock.Post(data);

                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex);
                            }
                        }
                    }
                    catch (OperationCanceledException ex)
                    {
                        // Ensure the consumer leaves the group cleanly and final offsets are committed.
                        consumer.Close();
                        _logger.LogError(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
        }

        public IConsumer<string, string> GetCurrentConsumer()
        {
            return consumer;
        }

        public void UpdateConsumerTopic(IEnumerable<string> topics)
        {
            consumer.Subscribe(topics);
        }
    }
}