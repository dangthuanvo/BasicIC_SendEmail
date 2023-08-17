using Confluent.Kafka;

namespace BasicIC_SendEmail.Models.Main
{
    public class ConsumerResultData
    {
        public ConsumerResultData(ConsumeResult<string, string> cr, IConsumer<string, string> consumer)
        {
            this.cr = cr;
            this.consumer = consumer;
        }

        public ConsumeResult<string, string> cr { get; set; }
        public IConsumer<string, string> consumer { get; set; }
    }
}
