﻿using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Web.ViewModels.Site
{
    public class PersonExtended : Person
    {
        public PersonExtended()
        {
        }

        public PersonExtended(Person person)
            : base(person)
        {
        }

        public IEnumerable<Unit> AnswerUnits { get; set; }

        public IEnumerable<AnswerType> AnswerTypes { get; set; }
    }
}