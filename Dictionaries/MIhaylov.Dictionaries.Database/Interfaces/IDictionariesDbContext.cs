using Microsoft.EntityFrameworkCore;
using Mihaylov.Common.Database.Interfaces;
using Mihaylov.Dictionaries.Database.Models;

namespace Mihaylov.Dictionaries.Database.Interfaces
{
    public interface IDictionariesDbContext : IDbContext
    {
        DbSet<AspNetUser> AspNetUsers { get; set; }

        DbSet<Course> Courses { get; set; }

        DbSet<Language> Languages { get; set; }

        DbSet<LearningSystem> LearningSystems { get; set; }

        DbSet<Level> Levels { get; set; }

        DbSet<PrepositionType> PrepositionTypes { get; set; }

        DbSet<Record> Records { get; set; }

        DbSet<RecordType> RecordTypes { get; set; }

        DbSet<Test> Tests { get; set; }
    }
}