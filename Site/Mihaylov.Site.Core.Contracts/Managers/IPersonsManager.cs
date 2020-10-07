using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.Interfaces
{
    public interface IPersonsManager
    {
        Task<IEnumerable<Person>> GetAllPersonsAsync(bool descOrder = false, int? pageNumber = null, int? pageSize = null);

        Person GetById(Guid id);

        Person GetByName(string name);

        Task<PersonStatistics> GetStaticticsAsync();
    }
}