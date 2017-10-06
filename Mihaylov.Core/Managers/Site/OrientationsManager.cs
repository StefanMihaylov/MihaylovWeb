using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Managers.Site
{
    public class OrientationsManager : IOrientationsManager
    {
        private readonly IOrientationsProvider provider;

        private readonly ConcurrentDictionary<int, Orientation> orientationsById;
        private readonly ConcurrentDictionary<string, Orientation> orientationsByName;

        public OrientationsManager(IOrientationsProvider orientationsProvider)
        {
            this.provider = orientationsProvider;
            this.orientationsById = new ConcurrentDictionary<int, Orientation>();
            this.orientationsByName = new ConcurrentDictionary<string, Orientation>(StringComparer.OrdinalIgnoreCase);

            Initialize();
        }

        public IEnumerable<Orientation> GetAllorientations()
        {
            IEnumerable<Orientation> orientations = this.orientationsById.Values;
            return orientations;
        }

        public Orientation GetById(int id)
        {
            Orientation orientation;
            if (this.orientationsById.TryGetValue(id, out orientation))
            {
                return orientation;
            }
            else
            {
                throw new ApplicationException($"Orientation with id: {id} was not found");
            }
        }

        public Orientation GetByName(string name)
        {
            name = name.Trim();
            Orientation orientation;
            if (this.orientationsByName.TryGetValue(name, out orientation))
            {
                return orientation;
            }
            else
            {
                throw new ApplicationException($"Orientation with name: {name} was not found");
            }
        }

        private void Initialize()
        {
            IEnumerable<Orientation> orientations = this.provider.GetAllOrientations();
            foreach (var orientation in orientations)
            {
                this.orientationsById.TryAdd(orientation.Id, orientation);
                this.orientationsByName.TryAdd(orientation.Name, orientation);
            }
        }
    }
}
