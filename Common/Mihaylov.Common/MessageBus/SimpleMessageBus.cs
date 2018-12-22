using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Common.MessageBus.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mihaylov.Common.MessageBus
{
    public class SimpleMessageBus : IMessageBus
    {
        private readonly ConcurrentDictionary<Type, IReadOnlyCollection<Action<Message>>> actions;

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

        public virtual void SendMessage(object data, object sender, MessageActionType actionType)
        {
            var message = new Message(data, sender, actionType);

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
