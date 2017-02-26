using System;
using System.Linq.Expressions;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Models.Repositories
{
    public class Country
    {
        public static Expression<Func<DAL.Country, Country>> FromDb
        {
            get
            {
                return country => new Country
                {
                    Id = country.CountryId,
                    Name = country.Name,
                    Description = country.Description,
                };
            }
        }

        public static implicit operator Country(DAL.Country countryDAL)
        {
            if (countryDAL == null)
            {
                return null;
            }

            Country countryDTO = new Country
            {
                Id = countryDAL.CountryId,
                Name = countryDAL.Name,
                Description = countryDAL.Description,
            };

            return countryDTO;
        }

        public DAL.Country Create()
        {
            DAL.Country country = new DAL.Country()
            {
                CountryId = this.Id,
                Name = this.Name,
                Description = this.Description,
            };

            return country;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
