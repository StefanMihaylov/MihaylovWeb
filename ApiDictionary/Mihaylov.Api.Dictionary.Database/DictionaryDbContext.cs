using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Dictionary.Database.DbConfigurations;
using Mihaylov.Api.Dictionary.Database.Models;
using Mihaylov.Common.Database.Interfaces;

namespace Mihaylov.Api.Dictionary.Database
{
    public class DictionaryDbContext : BaseDbContext<DictionaryDbContext>
    {
        public DictionaryDbContext(DbContextOptions<DictionaryDbContext> options,
            IAuditService auditService) : base(options, auditService)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Level> Levels { get; set; }

        public virtual DbSet<LearningSystem> LearningSystems { get; set; }

        public virtual DbSet<Language> Languages { get; set; }


        public virtual DbSet<Record> Records { get; set; }

        public virtual DbSet<RecordType> RecordTypes { get; set; }

        public virtual DbSet<Preposition> Prepositions { get; set; }


        // public virtual DbSet<Test> Tests { get; set; }

        // public virtual DbSet<IncorrectAnswer> IncorrectAnswers { get; set; }       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new LevelConfiguration());
            modelBuilder.ApplyConfiguration(new LearningSystemConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());

            modelBuilder.ApplyConfiguration(new RecordConfiguration());
            modelBuilder.ApplyConfiguration(new RecordTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PrepositionConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
