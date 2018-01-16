using System;
using System.Collections.Generic;
using System.Linq;
using Mihaylov.Core.Interfaces.Site;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Providers.Site
{
    public class PersonAdditionalInfoProvider : IPersonAdditionalInfoProvider
    {
        private readonly IGetAllRepository<AnswerType> answerTypeRepository;
        private readonly IGetAllRepository<Ethnicity> ethnicitiesRepository;
        private readonly IGetAllRepository<Orientation> orientationRepository;
        private readonly IGetAllRepository<Unit> unitRepository;
        private readonly ICountriesRepository countryRepository;

        public PersonAdditionalInfoProvider(
            IGetAllRepository<AnswerType> answerTypeRepository,
            IGetAllRepository<Ethnicity> ethnicitiesRepository,
            IGetAllRepository<Orientation> orientationRepository,
            IGetAllRepository<Unit> unitRepository,
            ICountriesRepository countryRepository)
        {
            this.answerTypeRepository = answerTypeRepository;
            this.ethnicitiesRepository = ethnicitiesRepository;
            this.orientationRepository = orientationRepository;
            this.unitRepository = unitRepository;
            this.countryRepository = countryRepository;
        }

        public PersonAdditionalInfoProvider()
        {
        }

        public IEnumerable<AnswerType> GetAllAnswerTypes()
        {
            IEnumerable<AnswerType> answerTypes = this.answerTypeRepository.GetAll()
                                                                           .ToList();
            return answerTypes;
        }

        public IEnumerable<Ethnicity> GetAllEthnicities()
        {
            IEnumerable<Ethnicity> ethnicities = this.ethnicitiesRepository.GetAll()
                                                                           .ToList();
            return ethnicities;
        }

        public IEnumerable<Orientation> GetAllOrientations()
        {
            IEnumerable<Orientation> orientations = orientationRepository.GetAll()
                                                                         .ToList();
            return orientations;
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            IEnumerable<Unit> units = this.unitRepository.GetAll()
                                                         .ToList();
            return units;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            IEnumerable<Country> countries = this.countryRepository.GetAll()
                                                                   .ToList();
            return countries;
        }

        public Country GetCountryById(int id)
        {
            Country country = this.countryRepository.GetById(id);

            if (country == null)
            {
                throw new ApplicationException($"Country with Id: {id} was not found");
            }

            return country;
        }

        public Country GetCountryByName(string name)
        {
            Country country = this.countryRepository.GetByName(name);

            if (country == null)
            {
                throw new ApplicationException($"Country with name: {name} was not found");
            }

            return country;
        }
    }
}
