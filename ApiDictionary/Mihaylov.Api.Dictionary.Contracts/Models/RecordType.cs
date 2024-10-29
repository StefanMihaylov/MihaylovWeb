using System.Collections.Generic;

namespace Mihaylov.Api.Dictionary.Contracts.Models
{
    public class RecordType
    {
        public const int NameMaxLength = 50;

        public int Id { get; set; }

        public string Name { get; set; }        
    }
}
