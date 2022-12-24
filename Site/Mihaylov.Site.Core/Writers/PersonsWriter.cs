using System;
using System.Threading.Tasks;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Writers
{
    public class PersonsWriter : IPersonsWriter
    {
        private readonly IPersonsRepository repository;
        // private readonly IMessageBus messageBus;

        public PersonsWriter(IPersonsRepository personsRepository)
        {
            this.repository = personsRepository;
            // this.messageBus = messageBus;
        }

        public async Task<Person> AddOrUpdateAsync(Person inputPerson)
        {
            if (inputPerson.LastBroadcastDate == default(DateTime))
            {
                inputPerson.LastBroadcastDate = inputPerson.AskDate.Value;
            }

            inputPerson.ModifiedOn = DateTime.UtcNow;

            Person person = await this.repository.AddOrUpdatePersonAsync(inputPerson);//, out bool isNewPerson);

            bool isNewPerson = false; // TODO;
            //MessageActionType action = isNewPerson ? MessageActionType.Add : MessageActionType.Update;
            //this.messageBus.SendMessage(person, this, action);

            return person;
        }
    }
}
