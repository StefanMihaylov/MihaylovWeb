using System.Data.Entity;
using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Database.Dictionaries;

namespace Mihaylov.Database.Interfaces
{
    public interface IDictionariesDbContext: IDbContext
    {
        DbSet<AspNetUser> AspNetUsers { get; set; }
        DbSet<Cours> Courses { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<LearningSystem> LearningSystems { get; set; }
        DbSet<Level> Levels { get; set; }
        DbSet<PrepositionType> PrepositionTypes { get; set; }
        DbSet<Record> Records { get; set; }
        DbSet<RecordType> RecordTypes { get; set; }
        DbSet<Test> Tests { get; set; }
    }
}