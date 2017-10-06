using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Providers.Site
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
