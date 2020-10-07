using System;
using System.Linq.Expressions;
using Mihaylov.Site.Data.Models.Base;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class Unit : LookupTable
    {
        public static Expression<Func<DAL.UnitType, Unit>> FromDb
        {
            get
            {
                return unit => new Unit
                {
                    Id = unit.Id,
                    Name = unit.Name,
                    Description = unit.Description,
                    ConversionRate = unit.ConversionRate,
                };
            }
        }

        public decimal ConversionRate { get; set; }
    }
}
