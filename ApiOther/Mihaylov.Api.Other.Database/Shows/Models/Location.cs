using System.Collections.Generic;
using Mihaylov.Common.Abstract.Databases.Models;

namespace Mihaylov.Api.Other.Database.Shows.Models
{
    public class Location : Entity
    {
        public int LocationId { get; set; }

        public string Name { get; set; }

        public IEnumerable<Concert> Concerts { get; set; }

        public Location()
        {
            Concerts = new List<Concert>();
        }
    }
}
