using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Writers;

namespace Mihaylov.Api.Site.Data.Writers
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
            //if (inputPerson.LastBroadcastDate == default(DateTime))
            //{
            //    inputPerson.LastBroadcastDate = inputPerson.AskDate.Value;
            //}

           // inputPerson.ModifiedOn = DateTime.UtcNow;

            Person person = await this.repository.AddOrUpdatePersonAsync(inputPerson);//, out bool isNewPerson);

            bool isNewPerson = false; // TODO;
            //MessageActionType action = isNewPerson ? MessageActionType.Add : MessageActionType.Update;
            //this.messageBus.SendMessage(person, this, action);

            return person;
        }
    }
}
