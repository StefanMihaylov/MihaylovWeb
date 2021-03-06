﻿using System;
using System.Linq.Expressions;
using DAL = Mihaylov.Database.Site;

namespace Mihaylov.Data.Models.Site
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
                    ConversionRate = unit.ConversionRate,
                };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal ConversionRate { get; set; }
    }
}
