using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Providers
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
