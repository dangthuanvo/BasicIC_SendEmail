using BasicIC_SendEmail.KafkaComponents;
using BasicIC_SendEmail.Models.Main;
using SocialConnection.KafkaComponents;
using System;
using System.Threading.Tasks;

namespace BasicIC_SendEmail.Common
{
    public class CommonFunc
    {
        public static async Task LogErrorToKafka(string topic, Exception ex)
        {
            //send log kafka
            LogSystemErrorModel logMess = new LogSystemErrorModel(ex, topic);
            ProducerWrapper<LogSystemErrorModel> _producer = new ProducerWrapper<LogSystemErrorModel>();
            await _producer.CreateMess(Topic.LOG_ERROR_SYSTEM, logMess);
        }
    }
}