using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IPersonsWriter
    {
        Person AddOrUpdate(Person inputPerson);
    }
}