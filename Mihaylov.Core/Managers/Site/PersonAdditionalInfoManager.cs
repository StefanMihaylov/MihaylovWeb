using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mihaylov.Common.Validations;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Ninject.Extensions.Logging;

namespace Mihaylov.Core.Managers.Site
{
    public class PersonAdditionalInfoManager : IPersonAdditionalInfoManager
    {
        protected readonly IPersonAdditionalInfoProvider provider;
        protected readonly ILogger logger;

        protected readonly ConcurrentDictionary<int, AnswerType> answerTypesById;
        protected readonly ConcurrentDictionary<string, AnswerType> answerTypesByName;

        protected readonly ConcurrentDictionary<int, Country> countriesById;
        protected readonly ConcurrentDictionary<string, Country> countriesByName;

        private readonly ConcurrentDictionary<int, Ethnicity> ethnicitiesById;
        private readonly ConcurrentDictionary<string, Ethnicity> ethnicitiesByName;

        private readonly ConcurrentDictionary<int, Orientation> orientationsById;
        private readonly ConcurrentDictionary<string, Orientation> orientationsByName;

        private readonly ConcurrentDictionary<int, Unit> unitsById;
        private readonly ConcurrentDictionary<string, Unit> unitsByName;

        public PersonAdditionalInfoManager(IPersonAdditionalInfoProvider provider, ILogger logger)
        {
            ParameterValidation.IsNotNull(provider, nameof(provider));
            ParameterValidation.IsNotNull(logger, nameof(logger));

            this.provider = provider;
            this.logger = logger;

            this.answerTypesById = new ConcurrentDictionary<int, AnswerType>();
            this.answerTypesByName = new ConcurrentDictionary<string, AnswerType>(StringComparer.OrdinalIgnoreCase);

            Initialize();

            this.countriesById = new ConcurrentDictionary<int, Country>();
            this.countriesByName = new ConcurrentDictionary<string, Country>(StringComparer.OrdinalIgnoreCase);

            this.ethnicitiesById = new ConcurrentDictionary<int, Ethnicity>();
            this.ethnicitiesByName = new ConcurrentDictionary<string, Ethnicity>(StringComparer.OrdinalIgnoreCase);

            InitializeEthnicity();

            this.orientationsById = new ConcurrentDictionary<int, Orientation>();
            this.orientationsByName = new ConcurrentDictionary<string, Orientation>(StringComparer.OrdinalIgnoreCase);

            InitializeOrientation();

            this.unitsById = new ConcurrentDictionary<int, Unit>();
            this.unitsByName = new ConcurrentDictionary<string, Unit>(StringComparer.OrdinalIgnoreCase);

            InitializeUnit();
        }

        public IEnumerable<AnswerType> GetAllAnswerTypes()
        {
            IEnumerable<AnswerType> answerTypes = this.answerTypesById.Values;
            return answerTypes;
        }

        public AnswerType GetAnswerTypeById(int id)
        {
            if (this.answerTypesById.TryGetValue(id, out AnswerType type))
            {
                return type;
            }
            else
            {
                throw new ApplicationException($"AnswerType with id: {id} was not found");
            }
        }

        public AnswerType GetAnswerTypeByName(string name)
        {
            if (this.answerTypesByName.TryGetValue(name.Trim(), out AnswerType type))
            {
                return type;
            }
            else
            {
                throw new ApplicationException($"AnswerType with name: {name} was not found");
            }
        }

        private void Initialize()
        {
            IEnumerable<AnswerType> answerTypes = this.provider.GetAllAnswerTypes();
            foreach (var answerType in answerTypes)
            {
                this.answerTypesById.TryAdd(answerType.Id, answerType);
                this.answerTypesByName.TryAdd(answerType.Name, answerType);
            }
        }

        public Country GetCountryById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("Id must be positive");
            }

            try
            {
                Country country = this.countriesById.GetOrAdd(id, (newId) =>
                {
                    Country newCountry = this.provider.GetCountryById(newId);
                    return newCountry;
                });

                return country;
            }
            catch (ApplicationException)
            {
                return null;
            }
        }

        public Country GetCountryByName(string name)
        {
            this.logger.Debug($"Manager: Get country by name: {name}");
            try
            {
                string key = name.Trim();
                Country country = this.countriesByName.GetOrAdd(key, (newName) =>
                {
                    this.logger.Debug($"Provider: Get country by name: {name}");

                    Country newCountry = this.provider.GetCountryByName(newName);

                    this.logger.Debug($"Provider: Found country by name ({name}): {newCountry?.Name}");

                    return newCountry;
                });

                return country;
            }
            catch (ApplicationException)
            {
                return null;
            }
        }

        public IEnumerable<Ethnicity> GetAllEthnicities()
        {
            IEnumerable<Ethnicity> ethnicities = this.ethnicitiesById.Values;
            return ethnicities;
        }

        public Ethnicity GetEthnicityById(int id)
        {
            if (this.ethnicitiesById.TryGetValue(id, out Ethnicity ethnicity))
            {
                return ethnicity;
            }
            else
            {
                throw new ApplicationException($"Ethnicity with id: {id} was not found");
            }
        }

        public Ethnicity GetEthnicityByName(string name)
        {
            if (this.ethnicitiesByName.TryGetValue(name.Trim(), out Ethnicity ethnicity))
            {
                return ethnicity;
            }
            else
            {
                throw new ApplicationException($"Ethnicity with name: {name} was not found");
            }
        }

        private void InitializeEthnicity()
        {
            IEnumerable<Ethnicity> ethnicities = this.provider.GetAllEthnicities();
            foreach (var ethnicity in ethnicities)
            {
                this.ethnicitiesById.TryAdd(ethnicity.Id, ethnicity);
                this.ethnicitiesByName.TryAdd(ethnicity.Name, ethnicity);
            }
        }

        public IEnumerable<Orientation> GetAllOrientations()
        {
            IEnumerable<Orientation> orientations = this.orientationsById.Values;
            return orientations;
        }

        public Orientation GetOrientationById(int id)
        {
            if (this.orientationsById.TryGetValue(id, out Orientation orientation))
            {
                return orientation;
            }
            else
            {
                throw new ApplicationException($"Orientation with id: {id} was not found");
            }
        }

        public Orientation GetOrientationByName(string name)
        {
            if (this.orientationsByName.TryGetValue(name.Trim(), out Orientation orientation))
            {
                return orientation;
            }
            else
            {
                throw new ApplicationException($"Orientation with name: {name} was not found");
            }
        }

        private void InitializeOrientation()
        {
            IEnumerable<Orientation> orientations = this.provider.GetAllOrientations();
            foreach (var orientation in orientations)
            {
                this.orientationsById.TryAdd(orientation.Id, orientation);
                this.orientationsByName.TryAdd(orientation.Name, orientation);
            }
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            IEnumerable<Unit> units = this.unitsById.Values;
            return units;
        }

        public Unit GetUnitById(int id)
        {
            if (this.unitsById.TryGetValue(id, out Unit unit))
            {
                return unit;
            }
            else
            {
                throw new ApplicationException($"Unit with id: {id} was not found");
            }
        }

        public Unit GetUnitByName(string name)
        {
            if (this.unitsByName.TryGetValue(name.Trim(), out Unit unit))
            {
                return unit;
            }
            else
            {
                throw new ApplicationException($"Unit with name: {name} was not found");
            }
        }

        private void InitializeUnit()
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
