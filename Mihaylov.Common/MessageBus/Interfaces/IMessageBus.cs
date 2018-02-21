using System;
using Mihaylov.Common.MessageBus.Models;

namespace Mihaylov.Common.MessageBus.Interfaces
{
    public interface IMessageBus
    {
        void Attach(Type type, Action<Message> action);

        void SendMessage(object data, object sender, MessageActionType actionType);
    }
}
