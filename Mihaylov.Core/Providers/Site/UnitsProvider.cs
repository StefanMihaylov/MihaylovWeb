using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Providers.Site
{
    public class UnitsProvider : IUnitsProvider
    {
        private readonly IGetAllRepository<Unit> repository;

        public UnitsProvider(IGetAllRepository<Unit> unitRepository)
        {
            this.repository = unitRepository;
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            IEnumerable<Unit> units = this.repository.GetAll()
                                                     .ToList();
            return units;
        }
    }
}
