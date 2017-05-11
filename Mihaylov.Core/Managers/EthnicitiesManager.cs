using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Managers
{
    public class EthnicitiesManager : IEthnicitiesManager
    {
        private readonly IEthnicitiesProvider provider;

        private readonly ConcurrentDictionary<int, Ethnicity> ethnicitiesById;
        private readonly ConcurrentDictionary<string, Ethnicity> ethnicitiesByName;

        public EthnicitiesManager(IEthnicitiesProvider unitProvider)
        {
            this.provider = unitProvider;
            this.ethnicitiesById = new ConcurrentDictionary<int, Ethnicity>();
            this.ethnicitiesByName = new ConcurrentDictionary<string, Ethnicity>(StringComparer.OrdinalIgnoreCase);

            Initialize();
        }

        public IEnumerable<Ethnicity> GetAllEthnicities()
        {
            IEnumerable<Ethnicity> ethnicities = this.ethnicitiesById.Values;
            return ethnicities;
        }

        public Ethnicity GetById(int id)
        {
            Ethnicity ethnicity;
            if (this.ethnicitiesById.TryGetValue(id, out ethnicity))
            {
                return ethnicity;
            }
            else
            {
                throw new ApplicationException($"Ethnicity with id: {id} was not found");
            }
        }

        public Ethnicity GetByName(string name)
        {
            name = name.Trim();
            Ethnicity ethnicity;
            if (this.ethnicitiesByName.TryGetValue(name, out ethnicity))
            {
                return ethnicity;
            }
            else
            {
                throw new ApplicationException($"Ethnicity with name: {name} was not found");
            }
        }

        private void Initialize()
        {
            IEnumerable<Ethnicity> ethnicities = this.provider.GetAllEthnicities();
            foreach (var ethnicity in ethnicities)
            {
                this.ethnicitiesById.TryAdd(ethnicity.Id, ethnicity);
                this.ethnicitiesByName.TryAdd(ethnicity.Name, ethnicity);
            }
        }
    }
}
