using System;
using System.Linq.Expressions;
using DAL = Mihaylov.Database.Site;

namespace Mihaylov.Data.Models.Site
{
    public class Orientation
    {
        public static Expression<Func<DAL.OrientationType, Orientation>> FromDb
        {
            get
            {
                return preference => new Orientation
                {
                    Id = preference.OrientationTypeId,
                    Name = preference.Name,
                    Description = preference.Description
                };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
