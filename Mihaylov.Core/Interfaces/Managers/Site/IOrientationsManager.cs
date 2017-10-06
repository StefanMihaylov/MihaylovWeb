using System.Collections.Generic;
using Mihaylov.Data.Models.Site;

namespace Mihaylov.Core.Interfaces.Site
{
    public interface IOrientationsManager
    {
        IEnumerable<Orientation> GetAllorientations();

        Orientation GetById(int id);

        Orientation GetByName(string name);
    }
}