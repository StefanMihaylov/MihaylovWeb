using System.Threading.Tasks;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Repositories;
using Mihaylov.Api.Site.Contracts.Writers;

namespace Mihaylov.Api.Site.Data.Writers
{
    public class PersonsWriter : IPersonsWriter
    {
        private readonly IPersonsRepository repository;

        public PersonsWriter(IPersonsRepository personsRepository)
        {
            this.repository = personsRepository;
        }

        public Task<Person> AddOrUpdateAsync(Person input)
        {
            return repository.AddOrUpdatePersonAsync(input);
        }
    }
}
