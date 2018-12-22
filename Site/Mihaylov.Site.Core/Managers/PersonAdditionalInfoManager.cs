using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.MessageBus;
using Mihaylov.Common.MessageBus.Interfaces;
using Mihaylov.Common.MessageBus.Models;
using Mihaylov.Common.Validations;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;
using Ninject.Extensions.Logging;

namespace Mihaylov.Site.Core.Managers
{
    public class PersonAdditionalInfoManager : IPersonAdditionalInfoManager
    {
        protected readonly IGetAllRepository<AnswerType> answerTypeRepository;
        protected readonly IGetAllRepository<Ethnicity> ethnicitiesRepository;
        protected readonly IGetAllRepository<Orientation> orientationRepository;
        protected readonly IGetAllRepository<Unit> unitRepository;
        protected readonly ICountriesRepository countryRepository;

        protected readonly ILogger logger;
        protected readonly IMessageBus messageBus;

        protected readonly Lazy<ConcurrentDictionary<int, AnswerType>> answerTypesById;
        protected readonly Lazy<ConcurrentDictionary<string, AnswerType>> answerTypesByName;

        private readonly Lazy<ConcurrentDictionary<int, Ethnicity>> ethnicitiesById;
        private readonly Lazy<ConcurrentDictionary<string, Ethnicity>> ethnicitiesByName;

        private readonly Lazy<ConcurrentDictionary<int, Orientation>> orientationsById;
        private readonly Lazy<ConcurrentDictionary<string, Orientation>> orientationsByName;

        private readonly Lazy<ConcurrentDictionary<int, Unit>> unitsById;
        private readonly Lazy<ConcurrentDictionary<string, Unit>> unitsByName;

        protected readonly ConcurrentDictionary<int, Country> countriesById;
        protected readonly ConcurrentDictionary<string, Country> countriesByName;

        public PersonAdditionalInfoManager(
            IGetAllRepository<AnswerType> answerTypeRepository,
            IGetAllRepository<Ethnicity> ethnicitiesRepository,
            IGetAllRepository<Orientation> orientationRepository,
            IGetAllRepository<Unit> unitRepository,
            ICountriesRepository countryRepository, 
            ILogger logger, IMessageBus messageBus)
        {
            ParameterValidation.IsNotNull(logger, nameof(logger));
            ParameterValidation.IsNotNull(messageBus, nameof(messageBus));

            this.answerTypeRepository = answerTypeRepository;
            this.ethnicitiesRepository = ethnicitiesRepository;
            this.orientationRepository = orientationRepository;
            this.unitRepository = unitRepository;
            this.countryRepository = countryRepository;

            this.logger = logger;
            this.messageBus = messageBus;

            this.answerTypesById = new Lazy<ConcurrentDictionary<int, AnswerType>>(() =>
            {
                IDictionary<int, AnswerType> answerTypes = this.answerTypeRepository.GetAll().ToDictionary(p => p.Id);
                return new ConcurrentDictionary<int, AnswerType>(answerTypes);
            });

            this.answerTypesByName = new Lazy<ConcurrentDictionary<string, AnswerType>>(() =>
            {
                IDictionary<string, AnswerType> answerTypes = this.answerTypeRepository.GetAll().ToDictionary(p => p.Name);
                return new ConcurrentDictionary<string, AnswerType>(answerTypes, StringComparer.OrdinalIgnoreCase);
            });

            this.ethnicitiesById = new Lazy<ConcurrentDictionary<int, Ethnicity>>(() =>
            {
                IDictionary<int, Ethnicity> ethnicities = this.ethnicitiesRepository.GetAll().ToDictionary(e => e.Id);
                return new ConcurrentDictionary<int, Ethnicity>(ethnicities);
            });

            this.ethnicitiesByName = new Lazy<ConcurrentDictionary<string, Ethnicity>>(() =>
            {
                IDictionary<string, Ethnicity> ethnicities = this.ethnicitiesRepository.GetAll().ToDictionary(e => e.Name);
                return new ConcurrentDictionary<string, Ethnicity>(ethnicities, StringComparer.OrdinalIgnoreCase);
            });

            this.orientationsById = new Lazy<ConcurrentDictionary<int, Orientation>>(() =>
            {
                IDictionary<int, Orientation> orientations = this.orientationRepository.GetAll().ToDictionary(o => o.Id);
                return new ConcurrentDictionary<int, Orientation>(orientations);
            });

            this.orientationsByName = new Lazy<ConcurrentDictionary<string, Orientation>>(() =>
            {
                IDictionary<string, Orientation> orientations = this.orientationRepository.GetAll().ToDictionary(o => o.Name);
                return new ConcurrentDictionary<string, Orientation>(orientations, StringComparer.OrdinalIgnoreCase);
            });

            this.unitsById = new Lazy<ConcurrentDictionary<int, Unit>>(() =>
            {
                IDictionary<int, Unit> units = this.unitRepository.GetAll().ToDictionary(u => u.Id);
                return new ConcurrentDictionary<int, Unit>(units);
            });
            this.unitsByName = new Lazy<ConcurrentDictionary<string, Unit>>(()=> 
            {
                IDictionary<string, Unit> units = this.unitRepository.GetAll().ToDictionary(u => u.Name);
                return new ConcurrentDictionary<string, Unit>(units, StringComparer.OrdinalIgnoreCase);
            }); 

            this.countriesById = new ConcurrentDictionary<int, Country>();
            this.countriesByName = new ConcurrentDictionary<string, Country>(StringComparer.OrdinalIgnoreCase);

            this.messageBus.Attach(typeof(Country), this.HandleMessageCountry);
        }

        #region AnswerType

        public IEnumerable<AnswerType> GetAllAnswerTypes()
        {
            IEnumerable<AnswerType> answerTypes = this.answerTypesById.Value.Values;
            return answerTypes;
        }

        public AnswerType GetAnswerTypeById(int id)
        {
            if (this.answerTypesById.Value.TryGetValue(id, out AnswerType type))
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
            if (this.answerTypesByName.Value.TryGetValue(name.Trim(), out AnswerType type))
            {
                return type;
            }
            else
            {
                throw new ApplicationException($"AnswerType with name: {name} was not found");
            }
        }

        #endregion

        #region Country

        public IEnumerable<Country> GetAllCountries()
        {
            return this.countriesById.Values;
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
                    Country newCountry = this.countryRepository.GetById(newId);
                    if (newCountry == null)
                    {
                        throw new ApplicationException($"Country with Id: {newId} was not found");
                    }
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

                    Country newCountry = this.countryRepository.GetByName(newName);
                    if (newCountry == null)
                    {
                        throw new ApplicationException($"Country with name: {newName} was not found");
                    }

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

        #endregion;

        #region Ethnicity

        public IEnumerable<Ethnicity> GetAllEthnicities()
        {
            IEnumerable<Ethnicity> ethnicities = this.ethnicitiesById.Value.Values;
            return ethnicities;
        }

        public Ethnicity GetEthnicityById(int id)
        {
            if (this.ethnicitiesById.Value.TryGetValue(id, out Ethnicity ethnicity))
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
            if (this.ethnicitiesByName.Value.TryGetValue(name.Trim(), out Ethnicity ethnicity))
            {
                return ethnicity;
            }
            else
            {
                throw new ApplicationException($"Ethnicity with name: {name} was not found");
            }
        }

        #endregion

        #region Orientation

        public IEnumerable<Orientation> GetAllOrientations()
        {
            IEnumerable<Orientation> orientations = this.orientationsById.Value.Values;
            return orientations;
        }

        public Orientation GetOrientationById(int id)
        {
            if (this.orientationsById.Value.TryGetValue(id, out Orientation orientation))
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
            if (this.orientationsByName.Value.TryGetValue(name.Trim(), out Orientation orientation))
            {
                return orientation;
            }
            else
            {
                throw new ApplicationException($"Orientation with name: {name} was not found");
            }
        }

        #endregion

        #region Unit

        public IEnumerable<Unit> GetAllUnits()
        {
            IEnumerable<Unit> units = this.unitsById.Value.Values;
            return units;
        }

        public Unit GetUnitById(int id)
        {
            if (this.unitsById.Value.TryGetValue(id, out Unit unit))
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
            if (this.unitsByName.Value.TryGetValue(name.Trim(), out Unit unit))
            {
                return unit;
            }
            else
            {
                throw new ApplicationException($"Unit with name: {name} was not found");
            }
        }

        #endregion

        private void HandleMessageCountry(Message message)
        {
            if (message == null)
            {
                return;
            }

            if (message.Data is Country country)
            {
                if (message.ActionType == MessageActionType.Add ||
                   (message.ActionType == MessageActionType.Update && this.countriesById.ContainsKey(country.Id)))
                {
                    this.countriesById.AddOrUpdate(country.Id, (id) => country, (updateId, existingCountry) => country);
                }

                if (message.ActionType == MessageActionType.Add ||
                    (message.ActionType == MessageActionType.Update && this.countriesByName.ContainsKey(country.Name)))
                {
                    this.countriesByName.AddOrUpdate(country.Name, (id) => country, (updateId, existingCountry) => country);
                }
            }
        }
    }
}
