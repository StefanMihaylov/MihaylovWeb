using Mihaylov.Common.MessageBus.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mihaylov.Common.MessageBus
{
    public class SimpleMessageBus : IMessageBus
    {
        private ConcurrentDictionary<Type, IReadOnlyCollection<Action<Message>>> actions;

        public SimpleMessageBus()
        {
            this.actions = new ConcurrentDictionary<Type, IReadOnlyCollection<Action<Message>>>();
        }

        public virtual void Attach(Type type, Action<Message> action)
        {
            this.actions.AddOrUpdate(type,
                (keyAdd) =>
                {
                    return new List<Action<Message>>() { action };
                },
                (keyUpdate, listUpdate) =>
                {
                    var list = listUpdate.ToList();
                    list.Add(action);
                    return list;
                });
        }

        public virtual void SendMessage(object data, object sender)
        {
            var message = new Message(data, this);

            if (this.actions.ContainsKey(message.MessageType))
            {
                if (this.actions.TryGetValue(message.MessageType, out IReadOnlyCollection<Action<Message>> actions))
                {
                    foreach (var action in actions)
                    {                        
                        action(message);
                    }
                }
            }
        }
    }
}
