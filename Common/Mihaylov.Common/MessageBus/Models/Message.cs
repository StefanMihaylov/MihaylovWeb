using System;
using System.Diagnostics;
using Mihaylov.Common.MessageBus.Models;

namespace Mihaylov.Common.MessageBus
{
    //[DebuggerStepThrough]
    public class Message
    {
        public Message(object data, object sender, MessageActionType actionType)
        {
            this.MessageId = Guid.NewGuid();
            this.SendDate = DateTime.Now;
            this.MessageType = data.GetType();
            this.Data = data;
            this.Sender = sender;
            this.ActionType = actionType;
        }

        public Guid MessageId { get; set; }

        public DateTime SendDate { get; set; }

        public Type MessageType { get; set; }

        public object Data { get; set; }

        public object Sender { get; set; }

        public MessageActionType ActionType { get; set; }
    }
}
