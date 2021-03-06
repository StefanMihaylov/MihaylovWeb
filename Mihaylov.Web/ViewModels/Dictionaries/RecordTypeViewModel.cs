﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Web.ViewModels.Dictionaries
{
    public class RecordTypeViewModel
    {
        public RecordTypeViewModel()
        {
        }

        public RecordTypeViewModel(RecordType recordType)
        {
            this.Id = recordType.Id;
            this.Name = recordType.Name;
            this.Selected = false;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }
    }
}