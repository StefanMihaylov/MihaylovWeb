using System;
using System.Linq.Expressions;
using Mihaylov.Site.Data.Models.Base;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class Country : LookupTable
    {
        public static Expression<Func<DAL.Country, Country>> FromDb
        {
            get
            {
                return country => new Country
                {
                    Id = country.Id,
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
                Id = countryDAL.Id,
                Name = countryDAL.Name,
                Description = countryDAL.Description,
            };

            return countryDTO;
        }

        public DAL.Country Create()
        {
            DAL.Country country = new DAL.Country()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
            };

            return country;
        }
    }
}
