using Microsoft.EntityFrameworkCore;
using Mihaylov.Api.Site.Database.DbConfigurations;
using Mihaylov.Api.Site.Database.Models;
using Mihaylov.Common.Database.Interfaces;

namespace Mihaylov.Api.Site.Database
{
    public class SiteDbContext : BaseDbContext<SiteDbContext>
    {
        public SiteDbContext(DbContextOptions<SiteDbContext> options, IAuditService auditService)
            : base(options, auditService)
        {
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<PersonDetail> PersonDetails { get; set; }

        public DbSet<PersonLocation> PersonLocations { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<CountryState> CountryStates { get; set; }

        public DbSet<Ethnicity> Ethnicities { get; set; }

        public DbSet<Orientation> Orientations { get; set; }

        public DbSet<DateOfBirth> DateOfBirthTypes { get; set; }


        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountType> AccountTypes { get; set; }

        public DbSet<AccountStatus> AccountStates { get; set; }


        public DbSet<MediaFile> MediaFiles { get; set; }

        public DbSet<MediaFileExtension> MediaFileExtensions { get; set; }

        public DbSet<MediaFileSource> MediaFileSources { get; set; }


        public DbSet<QuizPhrase> QuizPhrases { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<HalfType> HalfTypes { get; set; }

        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        public DbSet<QuizAnswer> QuizAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new PersonDetailConfiguration());
            modelBuilder.ApplyConfiguration(new PersonLocationConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CountryStateConfiguration());
            modelBuilder.ApplyConfiguration(new EthnicityConfiguration());
            modelBuilder.ApplyConfiguration(new OrientationConfiguration());
            modelBuilder.ApplyConfiguration(new AccountStatusConfiguration());
            modelBuilder.ApplyConfiguration(new AccountTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DateOfBirthConfiguration());
            modelBuilder.ApplyConfiguration(new MediaFileConfiguration());
            modelBuilder.ApplyConfiguration(new MediaFileExtensionConfiguration());
            modelBuilder.ApplyConfiguration(new MediaFileSourceConfiguration());
            modelBuilder.ApplyConfiguration(new UnitConfiguration());
            modelBuilder.ApplyConfiguration(new HalfTypeConfiguration());
            modelBuilder.ApplyConfiguration(new QuizPhraseConfiguration());
            modelBuilder.ApplyConfiguration(new QuizQuestionConfiguration());
            modelBuilder.ApplyConfiguration(new QuizAnswerConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
