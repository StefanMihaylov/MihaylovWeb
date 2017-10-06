namespace Mihaylov.Database.Interfaces
{
    using System.Data.Entity;
    using Mihaylov.Common.Database.Interfaces;
    using Mihaylov.Database.Site;

    public interface ISiteDbContext: IDbContext
    {
        DbSet<AnswerType> AnswerTypes { get; set; }

        DbSet<Country> Countries { get; set; }

        DbSet<EthnicityType> EthnicityTypes { get; set; }

        DbSet<OrientationType> OrientationTypes { get; set; }

        DbSet<Person> Persons { get; set; }

        DbSet<UnitType> UnitTypes { get; set; }
    }
}
