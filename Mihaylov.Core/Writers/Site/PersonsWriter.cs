using System;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Common.MessageBus.Models;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Writers.Site
{
    public class PersonsWriter : IPersonsWriter
    {
        private readonly IPersonsRepository repository;
        private readonly IMessageBus messageBus;

        public PersonsWriter(IPersonsRepository personsRepository, IMessageBus messageBus)
        {
            this.repository = personsRepository;
            this.messageBus = messageBus;
        }

        public Person AddOrUpdate(Person inputPerson)
        {
            if (inputPerson.LastBroadcastDate == default(DateTime))
            {
                inputPerson.LastBroadcastDate = inputPerson.AskDate.Value;
            }

            inputPerson.UpdatedDate = DateTime.UtcNow;

            Person person = this.repository.AddOrUpdatePerson(inputPerson, out bool isNewPerson);

            MessageActionType action = isNewPerson ? MessageActionType.Add : MessageActionType.Update;
            this.messageBus.SendMessage(person, this, action);

            return person;
        }
    }
}
