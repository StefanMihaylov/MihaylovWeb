using System;
using System.Diagnostics;

namespace Mihaylov.Common.MessageBus
{
    //[DebuggerStepThrough]
    public class Message
    {
        public Message(object data, object sender)
        {
            this.MessageId = Guid.NewGuid();
            this.SendDate = DateTime.Now;
            this.MessageType = data.GetType();
            this.Data = data;
            this.Sender = sender;          
        }

        public Guid MessageId { get; set; }

        public DateTime SendDate { get; set; }

        public Type MessageType { get; set; }

        public object Data { get; set; }

        public object Sender { get; set; }
    }
}
