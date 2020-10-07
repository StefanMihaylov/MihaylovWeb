using System;
using System.Linq.Expressions;
using Mihaylov.Site.Data.Models.Base;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class Ethnicity : LookupTable
    {
        public static Expression<Func<DAL.EthnicityType, Ethnicity>> FromDb
        {
            get
            {
                return ethnicity => new Ethnicity
                {
                    Id = ethnicity.Id,
                    Name = ethnicity.Name,
                    Description = ethnicity.Description
                };
            }
        }
    }
}
