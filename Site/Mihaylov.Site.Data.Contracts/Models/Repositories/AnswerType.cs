using System;
using System.Linq.Expressions;
using Mihaylov.Site.Data.Models.Base;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class AnswerType: LookupTable
    {
        public static Expression<Func<DAL.AnswerType, AnswerType>> FromDb
        {
            get
            {
                return type => new AnswerType
                {
                    Id = type.Id,
                    Name = type.Name,
                    Description = type.Description,
                    IsAsked = type.IsAsked,
                };
            }
        }

        public bool IsAsked { get; set; }
    }
}
