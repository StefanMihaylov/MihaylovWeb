using System;
using System.Linq.Expressions;
using DAL = Mihaylov.Database;

namespace Mihaylov.Data.Models.Repositories
{
    public class Unit
    {
        public static Expression<Func<DAL.UnitType, Unit>> FromDb
        {
            get
            {
                return unit => new Unit
                {
                    Id = unit.UnitTypeId,
                    Name = unit.Name,
                    Description = unit.Description,
                };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
