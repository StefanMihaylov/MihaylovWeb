using System;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Providers
{
    public class PersonsProvider : IPersonsProvider
    {
        private readonly IPersonsRepository repository;

        public PersonsProvider(IPersonsRepository personsRepository)
        {
            this.repository = personsRepository;
        }

        public IEnumerable<Person> GetAll(bool descOrder = false, int? pageNumber = null, int? pageSize = null)
        {
            IQueryable<Person> query = this.repository.GetAll().AsQueryable();

            if (descOrder)
            {
                query = query.OrderByDescending(p => p.AskDate);
            }
            else
            {
                query = query.OrderBy(p => p.AskDate);
            }

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                int skipCount = pageSize.Value * pageNumber.Value;
                query = query.Skip(skipCount).Take(pageSize.Value);
            }

            IEnumerable<Person> persons = query.ToList();
            return persons;
        }

        public Person GetById(int id)
        {
            Person person = this.repository.GetById(id);

            if (person == null)
            {
                throw new ApplicationException($"Person with Id: {id} was not found");
            }

            return person;
        }

        public Person GetByName(string name)
        {
            Person person = this.repository.GetByName(name.Trim());

            if (person == null)
            {
                throw new ApplicationException($"Person with name: {name} was not found");
            }

            return person;
        }

        public PersonStatistics GetStatictics()
        {
            PersonStatistics statistics = this.repository.GetStatictics();
            return statistics;
        }
    }
}
