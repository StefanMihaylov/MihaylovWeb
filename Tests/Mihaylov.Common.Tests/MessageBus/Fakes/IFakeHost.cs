using Mihaylov.Common.MessageBus;
using Mihaylov.Common.MessageBus.Interfaces;

namespace Mihaylov.Common.Tests.MessageBus.Fakes
{
    public interface IFakeHost
    {
        void Init(IMessageBus messageBus);

        void OnMessageOneReceived(Message message);

        void OnMessageOneReceivedExtended(Message message);

        void OnMessageTwoReceived(Message message);
    }
}
