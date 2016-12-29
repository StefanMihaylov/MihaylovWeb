namespace Mihaylov.Database.Models.Interfaces
{
    using System.Data.Entity;
    using Common.Interfaces;

    public interface IMihaylovDbContext: IDbContext
    {
        DbSet<Country> Countries { get; set; }
        DbSet<Ethnicity> Ethnicities { get; set; }
        DbSet<Person> Persons { get; set; }
        DbSet<SexPreference> SexPreferences { get; set; }
        DbSet<Unit> Units { get; set; }
    }
}
