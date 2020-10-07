using System;
using System.Linq.Expressions;
using Mihaylov.Site.Data.Models.Base;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class State : LookupTable
    {
        public static Expression<Func<DAL.State, State>> FromDb
        {
            get
            {
                return state => new State
                {
                    Id = state.Id,
                    Name = state.Name,
                    Description = state.Description,
                    CountryId = state.CountryId,
                };
            }
        }

        public int CountryId { get; set; }
    }    
}
