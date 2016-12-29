using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mihaylov.Common.Base;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Database.Models.Interfaces;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Repositories
{
    public class PersonsRepository : GenericRepository<DAL.Person, IMihaylovDbContext>
    {
        public PersonsRepository(IMihaylovDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Person> GetAll()
        {
            IEnumerable<Person> persons = this.All()
                                              .Select(Person.FromDb)
                                              .AsQueryable();

            return persons;
        }

        public Person GetById(int id)
        {
            Person person = this.All()
                                .Where(p => p.PersonId == id)
                                .Select(Person.FromDb)
                                .FirstOrDefault();

            return person;
        }
    }
}
