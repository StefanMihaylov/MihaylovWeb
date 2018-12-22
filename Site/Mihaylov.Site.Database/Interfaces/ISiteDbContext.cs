using Microsoft.EntityFrameworkCore;
using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Site.Database.Models;

namespace Mihaylov.Site.Database.Interfaces
{
    public interface ISiteDbContext : IDbContext
    {
        DbSet<AnswerType> AnswerTypes { get; set; }

        DbSet<Country> Countries { get; set; }

        DbSet<EthnicityType> EthnicityTypes { get; set; }

        DbSet<OrientationType> OrientationTypes { get; set; }

        DbSet<Person> Persons { get; set; }

        DbSet<UnitType> UnitTypes { get; set; }
    }
}
