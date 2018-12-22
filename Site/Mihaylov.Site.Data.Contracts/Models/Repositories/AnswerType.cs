using System;
using System.Linq.Expressions;
using DAL = Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Data.Models
{
    public class AnswerType
    {
        public static Expression<Func<DAL.AnswerType, AnswerType>> FromDb
        {
            get
            {
                return type => new AnswerType
                {
                    Id = type.AnswerTypeId,
                    Name = type.Name,
                    Description = type.Description,
                    IsAsked = type.IsAsked,
                };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsAsked { get; set; }
    }
}
