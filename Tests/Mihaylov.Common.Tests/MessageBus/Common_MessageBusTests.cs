using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Common.MessageBus;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Common.MessageBus.Models;
using Mihaylov.Common.Tests.MessageBus.Fakes;
using Moq;

namespace Mihaylov.Common.Tests.MessageBus
{
   // [TestClass]
    public class Common_MessageBusTests
    {
        public const string MESSAGE_STRING = "abc";
        public const int MESSAGE_INT = 123;
        public const decimal MESSAGE_UNKNOWN = 32.10m;

        [TestMethod]
        public void SendTwoMesssagesTest()
        {
            IMessageBus messageBus = new SimpleMessageBus();
            var host = GetFakeHostMocked(messageBus);

            host.Object.Init(messageBus);

            messageBus.SendMessage(MESSAGE_STRING, this, MessageActionType.Add);
            messageBus.SendMessage(MESSAGE_INT, this, MessageActionType.Add);

            host.Verify(x => x.OnMessageOneReceived(It.IsAny<Message>()), Times.Once());
            host.Verify(x => x.OnMessageOneReceivedExtended(It.IsAny<Message>()), Times.Exactly(2));
            host.Verify(x => x.OnMessageTwoReceived(It.IsAny<Message>()), Times.Once());
        }

        [TestMethod]
        public void SendStringMesssageOnlyTest()
        {
            IMessageBus messageBus = new SimpleMessageBus();
            var host = GetFakeHostMocked(messageBus);

            host.Object.Init(messageBus);

            messageBus.SendMessage(MESSAGE_STRING, this, MessageActionType.Add);

            host.Verify(x => x.OnMessageOneReceived(It.IsAny<Message>()), Times.Once());
            host.Verify(x => x.OnMessageOneReceivedExtended(It.IsAny<Message>()), Times.Once());
            host.Verify(x => x.OnMessageTwoReceived(It.IsAny<Message>()), Times.Never());
        }

        [TestMethod]
        public void SendIntMesssageOnlyTest()
        {
            IMessageBus messageBus = new SimpleMessageBus();
            var host = GetFakeHostMocked(messageBus);

            host.Object.Init(messageBus);

            messageBus.SendMessage(MESSAGE_INT, this, MessageActionType.Add);

            host.Verify(x => x.OnMessageOneReceived(It.IsAny<Message>()), Times.Never());
            host.Verify(x => x.OnMessageOneReceivedExtended(It.IsAny<Message>()), Times.Once());
            host.Verify(x => x.OnMessageTwoReceived(It.IsAny<Message>()), Times.Once());
        }

        [TestMethod]
        public void DontSendMesssageTest()
        {
            IMessageBus messageBus = new SimpleMessageBus();
            var host = GetFakeHostMocked(messageBus);

            host.Object.Init(messageBus);

            host.Verify(x => x.OnMessageOneReceived(It.IsAny<Message>()), Times.Never());
            host.Verify(x => x.OnMessageOneReceivedExtended(It.IsAny<Message>()), Times.Never());
            host.Verify(x => x.OnMessageTwoReceived(It.IsAny<Message>()), Times.Never());
        }

        [TestMethod]
        public void SendUnknownMesssageTest()
        {
            IMessageBus messageBus = new SimpleMessageBus();
            var host = GetFakeHostMocked(messageBus);

            host.Object.Init(messageBus);

            messageBus.SendMessage(MESSAGE_UNKNOWN, this, MessageActionType.Add);

            host.Verify(x => x.OnMessageOneReceived(It.IsAny<Message>()), Times.Never());
            host.Verify(x => x.OnMessageOneReceivedExtended(It.IsAny<Message>()), Times.Never());
            host.Verify(x => x.OnMessageTwoReceived(It.IsAny<Message>()), Times.Never());
        }

        private Mock<IFakeHost> GetFakeHostMocked(IMessageBus messageBus)
        {
            var host = new Mock<IFakeHost>();

            host.Setup(a => a.Init(messageBus)).Verifiable();
            host.Setup(a => a.OnMessageOneReceived(It.IsAny<Message>())).Verifiable();
            host.Setup(a => a.OnMessageTwoReceived(It.IsAny<Message>())).Verifiable(); 

            return host;
        }
    }
}
