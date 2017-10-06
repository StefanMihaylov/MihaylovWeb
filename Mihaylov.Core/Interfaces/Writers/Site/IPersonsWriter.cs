using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IPersonsWriter
    {
        Person Add(Person inputPerson);

        Person Update(Person updatedPerson);
    }
}