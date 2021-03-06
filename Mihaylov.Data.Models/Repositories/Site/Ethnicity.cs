﻿using System;
using System.Linq.Expressions;
using DAL = Mihaylov.Database.Site;

namespace Mihaylov.Data.Models.Site
{
    public class Ethnicity
    {
        public static Expression<Func<DAL.EthnicityType, Ethnicity>> FromDb
        {
            get
            {
                return ethnicity => new Ethnicity
                {
                    Id = ethnicity.EthnicityTypeId,
                    Name = ethnicity.Name,
                    Description = ethnicity.Description
                };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
