using System;

namespace Mihaylov.Common.MessageBus.Interfaces
{
    public interface IMessageBus
    {
        void Attach(Type type, Action<Message> action);

        void SendMessage(object data, object sender);
    }
}
