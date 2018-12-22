using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface IPersonsWriter
    {
        Person AddOrUpdate(Person inputPerson);
    }
}