using System;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Writers
{
    public class PersonsWriter : IPersonsWriter
    {
        private readonly IPersonsRepository repository;

        public PersonsWriter(IPersonsRepository personsRepository)
        {
            this.repository = personsRepository;
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
