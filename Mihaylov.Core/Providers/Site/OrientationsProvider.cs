using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Providers.Site
{
    public class OrientationsProvider : IOrientationsProvider
    {
        private readonly IGetAllRepository<Orientation> repository;

        public OrientationsProvider(IGetAllRepository<Orientation> orientationRepository)
        {
            this.repository = orientationRepository;
        }

        public IEnumerable<Orientation> GetAllOrientations()
        {
            IEnumerable<Orientation> orientations = repository.GetAll()
                                                              .ToList();
            return orientations;
        }
    }
}
