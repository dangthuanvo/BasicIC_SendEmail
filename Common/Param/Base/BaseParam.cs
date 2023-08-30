using System;
using System.Collections.Generic;

namespace Common.Params.Base
{
    public class BaseParam
    {
        public string tenant_id { get; set; }
    }

    public class TopicParam
    {
        public Object data { get; set; }
        public string topic { get; set; }
    }
    public class TopicSubscribe
    {
        public List<string> list_topic { get; set; }
    }
}
