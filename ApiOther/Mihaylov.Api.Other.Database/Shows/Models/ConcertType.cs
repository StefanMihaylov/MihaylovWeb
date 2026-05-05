using System.Collections.Generic;

namespace Mihaylov.Api.Other.Database.Shows.Models;

public class ConcertType
{
    public int ConcertTypeId { get; set; }

    public string Name { get; set; }

    public IEnumerable<Concert> Concerts { get; set; }

    public ConcertType()
    {
        Concerts = new List<Concert>();
    }
}
