using System;
using System.Linq.Expressions;
using Mihaylov.Site.Data.Models.Base;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class Orientation : LookupTable
    {
        public static Expression<Func<DAL.OrientationType, Orientation>> FromDb
        {
            get
            {
                return preference => new Orientation
                {
                    Id = preference.Id,
                    Name = preference.Name,
                    Description = preference.Description
                };
            }
        }
    }
}
