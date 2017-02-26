using System.Collections.Generic;
using Mihaylov.Data.Models.Repositories;

namespace Mihaylov.Core.Interfaces
{
    public interface IOrientationsProvider
    {
        IEnumerable<Orientation> GetAllOrientations();
    }
}