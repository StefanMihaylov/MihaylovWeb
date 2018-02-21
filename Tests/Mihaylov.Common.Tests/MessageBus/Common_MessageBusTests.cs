using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mihaylov.Common.MessageBus;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Common.MessageBus.Models;
using Mihaylov.Common.Tests.MessageBus.Fakes;
using Telerik.JustMock;

namespace Mihaylov.Common.Tests.MessageBus
{
    [TestClass]
    public class Common_MessageBusTests
    {
        public const string MESSAGE_STRING = "abc";
        public const int MESSAGE_INT = 123;
        public const decimal MESSAGE_UNKNOWN = 32.10m;

        [TestMethod]
        public void SendTwoMesssagesTest()
        {
            IMessageBus messageBus = new SimpleMessageBus();
            IFakeHost host = GetFakeHostMocked(messageBus);

            host.Init(messageBus);

            messageBus.SendMessage(MESSAGE_STRING, this, MessageActionType.Add);
            messageBus.SendMessage(MESSAGE_INT, this, MessageActionType.Add);

            Mock.Assert(() => host.OnMessageOneReceived(Arg.IsAny<Message>()), Occurs.Once());
            Mock.Assert(() => host.OnMessageOneReceivedExtended(Arg.IsAny<Message>()), Occurs.Exactly(2));
            Mock.Assert(() => host.OnMessageTwoReceived(Arg.IsAny<Message>()), Occurs.Once());
        }

        [TestMethod]
        public void SendStringMesssageOnlyTest()
        {
            IMessageBus messageBus = new SimpleMessageBus();
            IFakeHost host = GetFakeHostMocked(messageBus);

            host.Init(messageBus);

            messageBus.SendMessage(MESSAGE_STRING, this, MessageActionType.Add);

            Mock.Assert(() => host.OnMessageOneReceived(Arg.IsAny<Message>()), Occurs.Once());
            Mock.Assert(() => host.OnMessageOneReceivedExtended(Arg.IsAny<Message>()), Occurs.Once());
            Mock.Assert(() => host.OnMessageTwoReceived(Arg.IsAny<Message>()), Occurs.Never());
        }

        [TestMethod]
        public void SendIntMesssageOnlyTest()
        {
            IMessageBus messageBus = new SimpleMessageBus();
            IFakeHost host = GetFakeHostMocked(messageBus);

            host.Init(messageBus);

            messageBus.SendMessage(MESSAGE_INT, this, MessageActionType.Add);

            Mock.Assert(() => host.OnMessageOneReceived(Arg.IsAny<Message>()), Occurs.Never());
            Mock.Assert(() => host.OnMessageOneReceivedExtended(Arg.IsAny<Message>()), Occurs.Once());
            Mock.Assert(() => host.OnMessageTwoReceived(Arg.IsAny<Message>()), Occurs.Once());
        }

        [TestMethod]
        public void DontSendMesssageTest()
        {
            IMessageBus messageBus = new SimpleMessageBus();
            IFakeHost host = GetFakeHostMocked(messageBus);

            host.Init(messageBus);

            Mock.Assert(() => host.OnMessageOneReceived(Arg.IsAny<Message>()), Occurs.Never());
            Mock.Assert(() => host.OnMessageOneReceivedExtended(Arg.IsAny<Message>()), Occurs.Never());
            Mock.Assert(() => host.OnMessageTwoReceived(Arg.IsAny<Message>()), Occurs.Never());
        }

        [TestMethod]
        public void SendUnknownMesssageTest()
        {
            IMessageBus messageBus = new SimpleMessageBus();
            IFakeHost host = GetFakeHostMocked(messageBus);

            host.Init(messageBus);

            messageBus.SendMessage(MESSAGE_UNKNOWN, this, MessageActionType.Add);

            Mock.Assert(() => host.OnMessageOneReceived(Arg.IsAny<Message>()), Occurs.Never());
            Mock.Assert(() => host.OnMessageOneReceivedExtended(Arg.IsAny<Message>()), Occurs.Never());
            Mock.Assert(() => host.OnMessageTwoReceived(Arg.IsAny<Message>()), Occurs.Never());
        }

        private IFakeHost GetFakeHostMocked(IMessageBus messageBus)
        {
            IFakeHost host = Mock.Create<IFakeHost>();
            Mock.Arrange(() => host.Init(messageBus)).DoInstead((IMessageBus provider) =>
            {
                provider.Attach(MESSAGE_STRING.GetType(), host.OnMessageOneReceived);
                provider.Attach(MESSAGE_STRING.GetType(), host.OnMessageOneReceivedExtended);
                provider.Attach(MESSAGE_INT.GetType(), host.OnMessageTwoReceived);
                provider.Attach(MESSAGE_INT.GetType(), host.OnMessageOneReceivedExtended);
            });

            Mock.Arrange(() => host.OnMessageOneReceived(Arg.IsAny<Message>())).DoInstead((Message message) =>
            {
                Assert.AreEqual(MESSAGE_STRING.GetType(), message.MessageType);
                var messageRecieved = message.Data as string;
                Assert.AreEqual(MESSAGE_STRING, messageRecieved);
            });

            Mock.Arrange(() => host.OnMessageTwoReceived(Arg.IsAny<Message>())).DoInstead((Message message) =>
            {
                Assert.AreEqual(MESSAGE_INT.GetType(), message.MessageType);
                var messageRecieved = (int)message.Data;
                Assert.AreEqual(MESSAGE_INT, messageRecieved);
            });

            return host;
        }
    }
}
