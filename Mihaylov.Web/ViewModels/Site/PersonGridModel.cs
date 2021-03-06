﻿using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Web.ViewModels.Site
{
    public class PersonGridModel
    {
        public PersonStatistics Statistics { get; set; }

        public IEnumerable<Person> Persons { get; set; }

        public string SystemUnit { get; set; }

        public IEnumerable<Phrase> Phrases { get; set; }
    }
}