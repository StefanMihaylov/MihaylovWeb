﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mihaylov.Common.Databases.Models
{
    public abstract class Entity : IEntity
    {
        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}