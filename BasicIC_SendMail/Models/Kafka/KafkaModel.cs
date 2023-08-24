using BasicIC_SendEmail.Models.Main;
using System;
using System.Collections.Generic;

namespace BasicIC_SendEmail.Models.Kafka
{
    public class OrderDetailModel : BaseModel
    {
        public Guid order_id { get; set; }
        public Guid product_id { get; set; }
        public decimal product_price { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public decimal total_price { get; set; }
    }
    public class KafkaBaseModel
    {
        public Guid? id { get; set; }
        public DateTime? create_time { get; set; }
        public string create_by { get; set; }
        public DateTime? modify_time { get; set; }
        public string modify_by { get; set; }
        public Guid tenant_id { get; set; }
    }

    public class KafkaEmailModel : KafkaBaseModel
    {
        public string order_id { get; set; }
        public string customer_name { get; set; }
        public string customer_phone_number { get; set; }
        public decimal total_price { get; set; }
        public string shipping_address { get; set; }
        public int shipping_fee { get; set; }
        public List<OrderDetailModel> orderDetailModel { get; set; } = new List<OrderDetailModel>();
        public string to_email { get; set; }
        public string subject { get; set; }

        public KafkaEmailModel()
        {
            DateTime currentDateTime = DateTime.Now;
            create_time = currentDateTime;
            modify_time = currentDateTime;
            //id = Guid.NewGuid();
        }
    }

    //public class ListKafkaReceiveMessageModel
    //{
    //    public List<KafkaReceiveMessageModel> listKafkaModel { get; set; } = new List<KafkaReceiveMessageModel>();
    //    public string type { get; set; }
    //}

    //public class WebchatSendMessageModel : KafkaReceiveMessageModel
    //{
    //    [Required]
    //    public string url_website { get; set; }
    //    public string content_text_qa { get; set; }
    //}

    //public class KafkaReceiveStatusModel : KafkaBaseModel
    //{
    //    public KafkaReceiveStatusModel()
    //    {
    //        create_time = DateTime.Now;
    //    }
    //}

    //public class KafkaConversationMemberModel
    //{
    //    public string username { get; set; }
    //    public string user_state { get; set; }
    //}

    //public class KafkaWebhookMessageModel
    //{
    //    public string channel_type { get; set; }
    //    public string body { get; set; }
    //    public string header { get; set; }
    //    public string page_social_id { get; set; }
    //}


    //public class KafkaSaveMessageModel : KafkaBaseModel
    //{
    //    public bool is_agent_ic { get; set; }
    //    public bool is_active { get; set; }
    //    public List<KafkaConversationMemberModel> list_member { get; set; } = new List<KafkaConversationMemberModel>();
    //    public string from_user { get; set; }

    //    public KafkaSaveMessageModel()
    //    {
    //        id = Guid.NewGuid();
    //        var now = DateTime.Now;
    //        create_time = now;
    //        modify_time = now;
    //        sent_time = now;
    //    }
    //}
}