using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Providers
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
