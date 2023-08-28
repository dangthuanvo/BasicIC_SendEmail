using BasicIC_SendEmail.App_Start;
using BasicIC_SendEmail.Common;
using BasicIC_SendEmail.Config;
using BasicIC_SendEmail.KafkaComponents;
using BasicIC_SendEmail.Models.Common;
using BasicIC_SendEmail.Models.Kafka;
using BasicIC_SendEmail.Models.Main;
using BasicIC_SendEmail.Services.Interfaces;
using Common;
using Common.Commons;
using Common.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialConnection.KafkaComponents
{
    public class HandleProducer
    {
        private static readonly ILogger _logger = NinjectWebCommon.CreateInstanceDJ<ILogger>();
        private static readonly int maxErrorConfig = int.Parse(ConfigManager.StaticGet(Constants.CONF_MAX_ERROR_MESS));
        private static readonly ISendEmailService _sendEmailService = NinjectWebCommon.CreateInstanceDJ<ISendEmailService>();
        //private readonly IMessageProcessingService _messageProcessingService = NinjectWebCommon.CreateInstanceDJ<IMessageProcessingService>();
        //private readonly ISendEmailService _sendEmailService = NinjectWebCommon.CreateInstanceDJ<ISendEmailService>();
        private readonly List<TopicError> errors = new List<TopicError>();

        public HandleProducer()
        {

        }
        /// <summary>
        /// register handle topic
        /// </summary>
        /// <param name="data"></param>        
        /// <returns></returns>
        public async Task Register(ConsumerResultData data)
        {
            var response = new ResponseService<string>();
            var topic = data.cr.Topic;
            var mess = data.cr.Message.Value;

            switch (topic)
            {
                case Topic.SEND_EMAIL:
                    _logger.LogInfo("Receive topic [send_email] with data: " + mess);
                    var emailData = JsonConvert.DeserializeObject<ResponseMessage<KafkaEmailModel>>(mess);
                    await _sendEmailService.SendEmailOrderConfirm(emailData.data);
                    break;
            }

            // Handle commit
            if (response.ready_commit)
            {
                data.consumer.Commit(data.cr);
            }
            else
            {
            }

            // Handle exception
            if (response.exception != null)
            {
                await CommonFunc.LogErrorToKafka(topic, response.exception);
            }
        }

        /// <summary>
        /// update error topic
        /// </summary>
        /// <param name="topic"></param>
        private void UpdateError(string topic)
        {
            if (!(errors.Any(item => item.topic_name == topic)))
            {
                errors.Add(new TopicError { topic_name = topic, max_error = 1, total_error = 1 });
            }
            else
            {
                foreach (var item in errors)
                {
                    if (item.topic_name == topic)
                    {
                        item.max_error++;
                        item.total_error++;
                    }
                }
            }
        }

        /// <summary>
        /// number topic error can accepted
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        private bool isValidTopicError(string topic)
        {
            TopicError topicError = errors.Find(item => item.topic_name == topic);
            if (topicError != null && topicError.max_error >= maxErrorConfig) return false;
            return true;
        }
    }
}