using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface IPersonsWriter
    {
        Person Add(Person inputPerson);

        Person Update(Person updatedPerson);
    }
}