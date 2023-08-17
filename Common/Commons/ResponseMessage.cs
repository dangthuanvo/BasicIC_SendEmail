using Confluent.Kafka;
using System;
using System.Runtime.Serialization;

namespace Common.Commons
{
    public class ResponseMessage<T> : ResponseService<T>
    {
        [DataMember]
        public string topic { get; set; }
        [DataMember]
        public string tenant_id { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string token { get; set; }

        public ResponseMessage() : base() { }

        public ResponseMessage(string topic, Exception ex) : base(ex)
        {
            this.topic = topic;
        }

        public ResponseMessage(string topic, T data, string tenant_id = null) : base(data)
        {
            this.topic = topic;
            this.tenant_id = tenant_id;
        }

        /// <summary>
        /// return response error with message exception
        /// </summary>
        /// <param name="ex"></param>
        public ResponseMessage(string topic, KafkaException ex)
        {
            this.status = false;
            this.message = ex.Message;
            this.data = default;
            this.exception = ex;
            this.topic = topic;
        }
    }
}
