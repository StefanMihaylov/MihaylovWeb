using System;
using System.Collections.Generic;
using System.Text;

namespace Mihaylov.Api.Other.Contracts.Nexus.Models
{
    public class NexusImages
    {
        public IDictionary<string, IEnumerable<NexusImage>> Images { get; set; }
    }
}
