using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Base;
using Mihaylov.Data.Models.Interfaces;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Database.Models.Interfaces;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Repositories
{
    public class CountriesRepository : GenericRepository<DAL.Country, IMihaylovDbContext>, ICountriesRepository
    {
        public CountriesRepository(IMihaylovDbContext context) 
            : base(context)
        {
        }

        public IEnumerable<Country> GetAll()
        {
            IEnumerable<Country> ethnicities = this.All()
                                                   .Select(Country.FromDb)
                                                   .AsQueryable();
            return ethnicities;
        }

        public Country GetById(int id)
        {
            Country country = this.All()
                                  .Where(p => p.CountryId == id)
                                  .Select(Country.FromDb)
                                  .FirstOrDefault();
            return country;
        }

        public Country AddCountry(Country inputCountry)
        {
            DAL.Country country = inputCountry.Create();

            this.Add(country);
            this.Context.SaveChanges();

            return country;
        }
    }
}
