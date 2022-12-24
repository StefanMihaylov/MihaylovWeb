using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mihaylov.Common.Validations;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Managers
{
    public class CollectionsManager : ICollectionsManager
    {
        protected readonly ILookupTablesRepository _lookupTablesRepository;
        protected readonly ILocationsRepository _locationsRepository;

        protected readonly ILogger logger;
       // protected readonly IMessageBus messageBus;

        protected readonly Lazy<ConcurrentDictionary<int, AnswerType>> answerTypesById;
        protected readonly Lazy<ConcurrentDictionary<string, AnswerType>> answerTypesByName;

        private readonly Lazy<ConcurrentDictionary<int, Ethnicity>> ethnicitiesById;
        private readonly Lazy<ConcurrentDictionary<string, Ethnicity>> ethnicitiesByName;

        private readonly Lazy<ConcurrentDictionary<int, Orientation>> orientationsById;
        private readonly Lazy<ConcurrentDictionary<string, Orientation>> orientationsByName;

        protected readonly Lazy<ConcurrentDictionary<int, AccountType>> accountTypesById;
        protected readonly Lazy<ConcurrentDictionary<string, AccountType>> accountTypesByName;

        private readonly Lazy<ConcurrentDictionary<int, Unit>> unitsById;
        private readonly Lazy<ConcurrentDictionary<string, Unit>> unitsByName;

        protected readonly ConcurrentDictionary<int, Country> countriesById;
        protected readonly ConcurrentDictionary<string, Country> countriesByName;

        protected readonly Lazy<ConcurrentDictionary<int, IEnumerable<State>>> statesById;

        public CollectionsManager(
            ILookupTablesRepository lookupTablesRepository,
            ILocationsRepository locationsRepository,
            ILoggerFactory loggerFactory)
        {
            ParameterValidation.IsNotNull(loggerFactory, nameof(loggerFactory));
           // ParameterValidation.IsNotNull(messageBus, nameof(messageBus));

            this._lookupTablesRepository = lookupTablesRepository;
            this._locationsRepository = locationsRepository;

            this.logger = loggerFactory.CreateLogger(this.GetType().Name);
           // this.messageBus = messageBus;


            IEnumerable<AnswerType> answerTypeList = new List<AnswerType>();

            this.answerTypesById = new Lazy<ConcurrentDictionary<int, AnswerType>>(() =>
            {
                IDictionary<int, AnswerType> answerTypes = answerTypeList.ToDictionary(p => p.Id);
                return new ConcurrentDictionary<int, AnswerType>(answerTypes);
            });

            this.answerTypesByName = new Lazy<ConcurrentDictionary<string, AnswerType>>(() =>
            {
                IDictionary<string, AnswerType> answerTypes = answerTypeList.ToDictionary(p => p.Name);
                return new ConcurrentDictionary<string, AnswerType>(answerTypes, StringComparer.OrdinalIgnoreCase);
            });


            var ethnicityList = this._lookupTablesRepository.GetAllEthnicitiesAsync().GetAwaiter().GetResult();

            this.ethnicitiesById = new Lazy<ConcurrentDictionary<int, Ethnicity>>(() =>
            {
                IDictionary<int, Ethnicity> ethnicities = ethnicityList.ToDictionary(e => e.Id);
                return new ConcurrentDictionary<int, Ethnicity>(ethnicities);
            });

            this.ethnicitiesByName = new Lazy<ConcurrentDictionary<string, Ethnicity>>(() =>
            {
                IDictionary<string, Ethnicity> ethnicities = ethnicityList.ToDictionary(e => e.Name);
                return new ConcurrentDictionary<string, Ethnicity>(ethnicities, StringComparer.OrdinalIgnoreCase);
            });


            var orientationList = this._lookupTablesRepository.GetAllOrientationsAsync().GetAwaiter().GetResult();

            this.orientationsById = new Lazy<ConcurrentDictionary<int, Orientation>>(() =>
            {
                IDictionary<int, Orientation> orientations = orientationList.ToDictionary(o => o.Id);
                return new ConcurrentDictionary<int, Orientation>(orientations);
            });

            this.orientationsByName = new Lazy<ConcurrentDictionary<string, Orientation>>(() =>
            {
                IDictionary<string, Orientation> orientations = orientationList.ToDictionary(o => o.Name);
                return new ConcurrentDictionary<string, Orientation>(orientations, StringComparer.OrdinalIgnoreCase);
            });

            var accountTypeList = this._lookupTablesRepository.GetAllAccountTypesAsync().GetAwaiter().GetResult();

            this.accountTypesById = new Lazy<ConcurrentDictionary<int, AccountType>>(() =>
            {
                IDictionary<int, AccountType> accoutTypes = accountTypeList.ToDictionary(o => o.Id);
                return new ConcurrentDictionary<int, AccountType>(accoutTypes);
            });

            this.accountTypesByName = new Lazy<ConcurrentDictionary<string, AccountType>>(() =>
            {
                IDictionary<string, AccountType> accoutTypes = accountTypeList.ToDictionary(o => o.Name);
                return new ConcurrentDictionary<string, AccountType>(accoutTypes, StringComparer.OrdinalIgnoreCase);
            });

            IEnumerable<Unit> unitList = new List<Unit>();

            this.unitsById = new Lazy<ConcurrentDictionary<int, Unit>>(() =>
            {
                IDictionary<int, Unit> units = unitList.ToDictionary(u => u.Id);
                return new ConcurrentDictionary<int, Unit>(units);
            });

            this.unitsByName = new Lazy<ConcurrentDictionary<string, Unit>>(() =>
            {
                IDictionary<string, Unit> units = unitList.ToDictionary(u => u.Name);
                return new ConcurrentDictionary<string, Unit>(units, StringComparer.OrdinalIgnoreCase);
            });

            var countryList = this._locationsRepository.GetAllCountriesAsync().GetAwaiter().GetResult();
            
            var countries = countryList.ToDictionary(u => u.Id);
            this.countriesById = new ConcurrentDictionary<int, Country>(countries);

            this.countriesByName = new ConcurrentDictionary<string, Country>(StringComparer.OrdinalIgnoreCase);

           // this.messageBus.Attach(typeof(Country), this.HandleMessageCountry);

            var stateDictionary = this._locationsRepository.GetAllStatesAsync().GetAwaiter().GetResult();

            this.statesById = new Lazy<ConcurrentDictionary<int, IEnumerable<State>>>(() =>
            {
                return new ConcurrentDictionary<int, IEnumerable<State>>(stateDictionary);
            });
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
                    Country newCountry = this._locationsRepository.GetCountryByIdAsync(newId).ConfigureAwait(false).GetAwaiter().GetResult();
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
            this.logger.LogDebug($"Manager: Get country by name: {name}");
            try
            {
                string key = name.Trim();
                Country country = this.countriesByName.GetOrAdd(key, (newName) =>
                {
                    this.logger.LogDebug($"Provider: Get country by name: {name}");

                    Country newCountry = this._locationsRepository.GetCountryByNameAsync(newName).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (newCountry == null)
                    {
                        throw new ApplicationException($"Country with name: {newName} was not found");
                    }

                    this.logger.LogDebug($"Provider: Found country by name ({name}): {newCountry?.Name}");

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

        #region State

        public IEnumerable<State> GetAllStatesByCountryId(int countryId)
        {
            if (this.statesById.Value.TryGetValue(countryId, out IEnumerable<State> states))
            {
                return states;
            }
            else
            {
                return new List<State>();
            }
        }

        #endregion

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

        #region AccountType

        public IEnumerable<AccountType> GetAllAccountTypes()
        {
            IEnumerable<AccountType> accountTypes = this.accountTypesById.Value.Values;
            return accountTypes;
        }

        public AccountType GetAccountTypeById(int id)
        {
            if (this.accountTypesById.Value.TryGetValue(id, out AccountType accountType))
            {
                return accountType;
            }
            else
            {
                throw new ApplicationException($"AccountType with id: {id} was not found");
            }
        }

        public AccountType GetAccountTypeByName(string name)
        {
            if (this.accountTypesByName.Value.TryGetValue(name.Trim(), out AccountType accountType))
            {
                return accountType;
            }
            else
            {
                throw new ApplicationException($"AccountType with name: {name} was not found");
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

        //private void HandleMessageCountry(Message message)
        //{
        //    if (message == null)
        //    {
        //        return;
        //    }

        //    if (message.Data is Country country)
        //    {
        //        if (message.ActionType == MessageActionType.Add ||
        //           (message.ActionType == MessageActionType.Update && this.countriesById.ContainsKey(country.Id)))
        //        {
        //            this.countriesById.AddOrUpdate(country.Id, (id) => country, (updateId, existingCountry) => country);
        //        }

        //        if (message.ActionType == MessageActionType.Add ||
        //            (message.ActionType == MessageActionType.Update && this.countriesByName.ContainsKey(country.Name)))
        //        {
        //            this.countriesByName.AddOrUpdate(country.Name, (id) => country, (updateId, existingCountry) => country);
        //        }
        //    }
        //}
    }
}
