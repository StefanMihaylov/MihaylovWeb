namespace Mihaylov.Database.Models.Interfaces
{
    using System.Data.Entity;
    using Common.Interfaces;

    public interface IMihaylovDbContext: IDbContext
    {
        DbSet<AnswerType> AnswerTypes { get; set; }

        DbSet<Country> Countries { get; set; }

        DbSet<EthnicityType> EthnicityTypes { get; set; }

        DbSet<OrientationType> OrientationTypes { get; set; }

        DbSet<Person> Persons { get; set; }

        DbSet<UnitType> UnitTypes { get; set; }
    }
}
