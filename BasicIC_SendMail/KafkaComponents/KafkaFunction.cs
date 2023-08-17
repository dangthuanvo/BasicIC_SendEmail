using BasicIC_SendEmail.Models.Kafka;
using SocialConnection.KafkaComponents;
using System;
using System.Threading.Tasks;

namespace BasicIC_SendEmail.KafkaComponents
{
    public static class KafkaFunction<T> where T : KafkaBaseModel
    {
        private static readonly ProducerWrapper<T> _producer = new ProducerWrapper<T>();

        public static async Task PushToKafkaAsync(string topic, T model)
        {
            if (!model.id.HasValue)
                model.id = Guid.NewGuid();
            await _producer.CreateMess(topic, model, model.tenant_id.ToString());
        }
    }

    public static class KafkaFunctionNonDefinedClass<T> where T : class
    {
        private static readonly ProducerWrapper<T> _producer = new ProducerWrapper<T>();

        public static async Task PushToKafkaAsync(string topic, T model)
        {
            await _producer.CreateMess(topic, model);
        }
    }
}