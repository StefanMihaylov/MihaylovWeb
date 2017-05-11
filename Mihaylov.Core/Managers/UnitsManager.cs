using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mihaylov.Core.Interfaces;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Managers
{
    public class UnitsManager : IUnitsManager
    {
        private readonly IUnitsProvider provider;

        private readonly ConcurrentDictionary<int, Unit> unitsById;
        private readonly ConcurrentDictionary<string, Unit> unitsByName;

        public UnitsManager(IUnitsProvider unitProvider)
        {
            this.provider = unitProvider;
            this.unitsById = new ConcurrentDictionary<int, Unit>();
            this.unitsByName = new ConcurrentDictionary<string, Unit>(StringComparer.OrdinalIgnoreCase);

            Initialize();
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            IEnumerable<Unit> units = this.unitsById.Values;
            return units;
        }

        public Unit GetById(int id)
        {            
            Unit unit;
            if (this.unitsById.TryGetValue(id, out unit))
            {
                return unit;
            }
            else
            {
                throw new ApplicationException($"Unit with id: {id} was not found");
            }
        }

        public Unit GetByName(string name)
        {
            name = name.Trim();
            Unit unit;
            if (this.unitsByName.TryGetValue(name, out unit))
            {
                return unit;
            }
            else
            {
                throw new ApplicationException($"Unit with name: {name} was not found");
            }
        }

        private void Initialize()
        {
            IEnumerable<Unit> units = this.provider.GetAllUnits();
            foreach (var unit in units)
            {
                this.unitsById.TryAdd(unit.Id, unit);
                this.unitsByName.TryAdd(unit.Name, unit);
            }
        }
    }
}
