using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Providers
{
    public class EthnicitiesProvider : IEthnicitiesProvider
    {
        private readonly IGetAllRepository<Ethnicity> repository;

        public EthnicitiesProvider(IGetAllRepository<Ethnicity> ethnicitiesRepository)
        {
            this.repository = ethnicitiesRepository;
        }

        public IEnumerable<Ethnicity> GetAllEthnicities()
        {
            IEnumerable<Ethnicity> ethnicities = this.repository.GetAll()
                                                                .ToList();
            return ethnicities;
        }
    }
}
