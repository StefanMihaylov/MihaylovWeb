using System;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Writers.Site
{
    public class PersonsWriter : IPersonsWriter
    {
        private readonly IPersonsRepository repository;
        private readonly IUnitsManager unitsManager;

        public PersonsWriter(IPersonsRepository personsRepository, IUnitsManager unitsManager)
        {
            this.repository = personsRepository;
            this.unitsManager = unitsManager;
        }

        public Person Add(Person inputPerson)
        {
            if (inputPerson.LastBroadcastDate == default(DateTime))
            {
                inputPerson.LastBroadcastDate = inputPerson.AskDate.Value;
            }

            inputPerson.UpdatedDate = DateTime.Now;

            Person person = this.repository.AddPerson(inputPerson);
            return person;
        }

        public Person Update(Person updatedPerson)
        {
            updatedPerson.UpdatedDate = DateTime.Now;

            Person person = this.repository.UpdatePerson(updatedPerson);
            return person;
        }
    }
}
