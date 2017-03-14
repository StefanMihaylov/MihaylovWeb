using System;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Writers
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

            Person person = this.repository.AddPerson(inputPerson);
            return person;
        }
    }
}
