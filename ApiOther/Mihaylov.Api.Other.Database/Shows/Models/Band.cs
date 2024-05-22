using System.Collections.Generic;
using Mihaylov.Common.Abstract.Databases.Models;

namespace Mihaylov.Api.Other.Database.Shows.Models
{
    public class Band : Entity
    {
        public int BandId { get; set; }

        public string Name { get; set; }

        public IEnumerable<Concert> Concerts { get; set; }

        public IEnumerable<ConcertBand> ConcertBands { get; set; }

        public Band()
        {
            Concerts = new List<Concert>();
            ConcertBands = new List<ConcertBand>();
        }
    }
}
