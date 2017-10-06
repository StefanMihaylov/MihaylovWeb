using System.Collections.Generic;
using System.Linq;
using Mihaylov.Common.Database;
using Mihaylov.Data.Interfaces.Site;
using Mihaylov.Data.Models.Site;
using Mihaylov.Database.Interfaces;
using DAL = Mihaylov.Database.Site;

namespace Mihaylov.Data.Repositories.Site
{
    public class CountriesRepository : GenericRepository<DAL.Country, ISiteDbContext>, ICountriesRepository
    {
        public CountriesRepository(ISiteDbContext context)
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

        public Country GetByName(string name)
        {
            var filtered = this.All().Where(p => p.Name == name).ToList();

            Country country = this.All()
                                  .Where(p => p.Name == name)
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
