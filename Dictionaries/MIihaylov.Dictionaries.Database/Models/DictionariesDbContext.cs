using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Mihaylov.Dictionaries.Database.Models
{
    public partial class DictionariesDbContext : DbContext
    {
        public DictionariesDbContext()
        {
        }

        public DictionariesDbContext(DbContextOptions<DictionariesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<IncorrectAnswer> IncorrectAnswers { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LearningSystem> LearningSystems { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<PrepositionType> PrepositionTypes { get; set; }
        public virtual DbSet<RecordRecordType> RecordRecordTypes { get; set; }
        public virtual DbSet<RecordType> RecordTypes { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        public virtual DbSet<Test> Tests { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=.;Database=MihaylovDb_copy;Trusted_Connection=True;");
//            }
//        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

        //    modelBuilder.Entity<AnswerType>(entity =>
        //    {
        //        entity.HasKey(e => e.AnswerTypeId)
        //            .HasName("PK_Answers");

        //        entity.ToTable("AnswerTypes", "cam");

        //        entity.HasIndex(e => e.Name)
        //            .HasName("UX_AnswerTypes_Name")
        //            .IsUnique();

        //        entity.Property(e => e.Description).HasMaxLength(100);

        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(50);
        //    });

        //    modelBuilder.Entity<AspNetRoles>(entity =>
        //    {
        //        entity.HasIndex(e => e.Name)
        //            .HasName("RoleNameIndex")
        //            .IsUnique();

        //        entity.Property(e => e.Id)
        //            .HasMaxLength(128)
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(256);
        //    });

        //    modelBuilder.Entity<AspNetUserClaims>(entity =>
        //    {
        //        entity.HasIndex(e => e.UserId)
        //            .HasName("IX_UserId");

        //        entity.Property(e => e.UserId)
        //            .IsRequired()
        //            .HasMaxLength(128);

        //        entity.HasOne(d => d.User)
        //            .WithMany(p => p.AspNetUserClaims)
        //            .HasForeignKey(d => d.UserId)
        //            .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
        //    });

        //    modelBuilder.Entity<AspNetUserLogins>(entity =>
        //    {
        //        entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
        //            .HasName("PK_dbo.AspNetUserLogins");

        //        entity.HasIndex(e => e.UserId)
        //            .HasName("IX_UserId");

        //        entity.Property(e => e.LoginProvider).HasMaxLength(128);

        //        entity.Property(e => e.ProviderKey).HasMaxLength(128);

        //        entity.Property(e => e.UserId).HasMaxLength(128);

        //        entity.HasOne(d => d.User)
        //            .WithMany(p => p.AspNetUserLogins)
        //            .HasForeignKey(d => d.UserId)
        //            .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
        //    });

        //    modelBuilder.Entity<AspNetUserRoles>(entity =>
        //    {
        //        entity.HasKey(e => new { e.UserId, e.RoleId })
        //            .HasName("PK_dbo.AspNetUserRoles");

        //        entity.HasIndex(e => e.RoleId)
        //            .HasName("IX_RoleId");

        //        entity.HasIndex(e => e.UserId)
        //            .HasName("IX_UserId");

        //        entity.Property(e => e.UserId).HasMaxLength(128);

        //        entity.Property(e => e.RoleId).HasMaxLength(128);

        //        entity.HasOne(d => d.Role)
        //            .WithMany(p => p.AspNetUserRoles)
        //            .HasForeignKey(d => d.RoleId)
        //            .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

        //        entity.HasOne(d => d.User)
        //            .WithMany(p => p.AspNetUserRoles)
        //            .HasForeignKey(d => d.UserId)
        //            .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
        //    });

        //    modelBuilder.Entity<AspNetUsers>(entity =>
        //    {
        //        entity.HasIndex(e => e.UserName)
        //            .HasName("UserNameIndex")
        //            .IsUnique();

        //        entity.Property(e => e.Id)
        //            .HasMaxLength(128)
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.Email).HasMaxLength(256);

        //        entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

        //        entity.Property(e => e.UserName)
        //            .IsRequired()
        //            .HasMaxLength(256);
        //    });

        //    modelBuilder.Entity<Countries>(entity =>
        //    {
        //        entity.HasKey(e => e.CountryId)
        //            .HasName("PK__Countrie__10D1609F3F5E89FD");

        //        entity.ToTable("Countries", "cam");

        //        entity.HasIndex(e => e.Name)
        //            .HasName("UX_Countries_Name")
        //            .IsUnique();

        //        entity.Property(e => e.Description).HasMaxLength(100);

        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(50);
        //    });

        //    modelBuilder.Entity<Course>(entity =>
        //    {
        //        entity.HasKey(e => e.CourseId);

        //        entity.ToTable("Courses", "dict");

        //        entity.Property(e => e.EndDate).HasColumnType("date");

        //        entity.Property(e => e.StartDate).HasColumnType("date");

        //        entity.HasOne(d => d.Level)
        //            .WithMany(p => p.Courses)
        //            .HasForeignKey(d => d.LevelId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Courses_Levels");
        //    });

        //    modelBuilder.Entity<EthnicityTypes>(entity =>
        //    {
        //        entity.HasKey(e => e.EthnicityTypeId)
        //            .HasName("PK__Ethnicit__4E8746DBD5F2C2C8");

        //        entity.ToTable("EthnicityTypes", "cam");

        //        entity.HasIndex(e => e.Name)
        //            .HasName("UX_EthnicityTypes_Name")
        //            .IsUnique();

        //        entity.Property(e => e.Description).HasMaxLength(100);

        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(50);
        //    });

        //    modelBuilder.Entity<IncorrectAnswer>(entity =>
        //    {
        //        entity.HasKey(e => new { e.TestId, e.RecordId })
        //            .HasName("PK_IncorrectAnswers_1");

        //        entity.ToTable("IncorrectAnswers", "dict");

        //        entity.HasOne(d => d.Record)
        //            .WithMany(p => p.IncorrectAnswers)
        //            .HasForeignKey(d => d.RecordId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_IncorrectAnswers_Records");

        //        entity.HasOne(d => d.Test)
        //            .WithMany(p => p.IncorrectAnswers)
        //            .HasForeignKey(d => d.TestId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_IncorrectAnswers_Tests");
        //    });

        //    modelBuilder.Entity<Language>(entity =>
        //    {
        //        entity.HasKey(e => e.LanguageId);

        //        entity.ToTable("Languages", "dict");

        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(50);
        //    });

        //    modelBuilder.Entity<LearningSystem>(entity =>
        //    {
        //        entity.HasKey(e => e.LearningSystemId);

        //        entity.ToTable("LearningSystems", "dict");

        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(50);

        //        entity.HasOne(d => d.Language)
        //            .WithMany(p => p.LearningSystems)
        //            .HasForeignKey(d => d.LanguageId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_LearningSystems_Languages");
        //    });

        //    modelBuilder.Entity<Level>(entity =>
        //    {
        //        entity.HasKey(e => e.LevelId);

        //        entity.ToTable("Levels", "dict");

        //        entity.Property(e => e.Descrition)
        //            .IsRequired()
        //            .HasMaxLength(100);

        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(10);

        //        entity.HasOne(d => d.LearningSystem)
        //            .WithMany(p => p.Levels)
        //            .HasForeignKey(d => d.LearningSystemId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Levels_LearningSystems");
        //    });

        //    modelBuilder.Entity<OrientationTypes>(entity =>
        //    {
        //        entity.HasKey(e => e.OrientationTypeId)
        //            .HasName("PK__Orientat__691D88C85A4A66C9");

        //        entity.ToTable("OrientationTypes", "cam");

        //        entity.HasIndex(e => e.Name)
        //            .HasName("UX_OrientationTypes_Name")
        //            .IsUnique();

        //        entity.Property(e => e.Description).HasMaxLength(100);

        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(50);
        //    });

        //    modelBuilder.Entity<Persons>(entity =>
        //    {
        //        entity.HasKey(e => e.PersonId)
        //            .HasName("PK__Persons__AA2FFBE59AB59249");

        //        entity.ToTable("Persons", "cam");

        //        entity.HasIndex(e => e.Username)
        //            .HasName("UX_Persons_Username")
        //            .IsUnique();

        //        entity.Property(e => e.Answer).HasColumnType("decimal(5, 2)");

        //        entity.Property(e => e.AnswerConverted).HasColumnType("decimal(5, 2)");

        //        entity.Property(e => e.AskDate).HasColumnType("datetime");

        //        entity.Property(e => e.Comments).HasMaxLength(128);

        //        entity.Property(e => e.CreateDate).HasColumnType("date");

        //        entity.Property(e => e.LastBroadcastDate).HasColumnType("datetime");

        //        entity.Property(e => e.RecordsPath).HasMaxLength(128);

        //        entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

        //        entity.Property(e => e.Username)
        //            .IsRequired()
        //            .HasMaxLength(50);

        //        entity.HasOne(d => d.AnswerType)
        //            .WithMany(p => p.Persons)
        //            .HasForeignKey(d => d.AnswerTypeId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Persons_AnswerTypes");

        //        entity.HasOne(d => d.AnswerUnitType)
        //            .WithMany(p => p.Persons)
        //            .HasForeignKey(d => d.AnswerUnitTypeId)
        //            .HasConstraintName("FK_Persons_UnitTypes");

        //        entity.HasOne(d => d.Country)
        //            .WithMany(p => p.Persons)
        //            .HasForeignKey(d => d.CountryId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Persons_Countries");

        //        entity.HasOne(d => d.EthnicityType)
        //            .WithMany(p => p.Persons)
        //            .HasForeignKey(d => d.EthnicityTypeId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Persons_EthnicityTypes");

        //        entity.HasOne(d => d.OrientationType)
        //            .WithMany(p => p.Persons)
        //            .HasForeignKey(d => d.OrientationTypeId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Persons_OrientationTypes");
        //    });

        //    modelBuilder.Entity<Phrases>(entity =>
        //    {
        //        entity.HasKey(e => e.PhraseId);

        //        entity.ToTable("Phrases", "cam");

        //        entity.HasIndex(e => e.OrderId)
        //            .HasName("NonClusteredIndex-OrderId-Unique")
        //            .IsUnique();

        //        entity.Property(e => e.Text)
        //            .IsRequired()
        //            .HasMaxLength(200);
        //    });

        //    modelBuilder.Entity<PrepositionType>(entity =>
        //    {
        //        entity.HasKey(e => e.PrepositionTypeId);

        //        entity.ToTable("PrepositionTypes", "dict");

        //        entity.Property(e => e.Value)
        //            .IsRequired()
        //            .HasMaxLength(50);

        //        entity.HasOne(d => d.Language)
        //            .WithMany(p => p.PrepositionTypes)
        //            .HasForeignKey(d => d.LanguageId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_PrepositionTypes_Languages");
        //    });

        //    modelBuilder.Entity<RecordRecordType>(entity =>
        //    {
        //        entity.HasKey(e => new { e.RecordId, e.RecordTypeId });

        //        entity.ToTable("RecordRecordTypes", "dict");

        //        entity.HasOne(d => d.Record)
        //            .WithMany(p => p.RecordRecordTypes)
        //            .HasForeignKey(d => d.RecordId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_RecordRecordTypes_Records");

        //        entity.HasOne(d => d.RecordType)
        //            .WithMany(p => p.RecordRecordTypes)
        //            .HasForeignKey(d => d.RecordTypeId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_RecordRecordTypes_RecordTypes");
        //    });

        //    modelBuilder.Entity<RecordType>(entity =>
        //    {
        //        entity.HasKey(e => e.RecordTypeId);

        //        entity.ToTable("RecordTypes", "dict");

        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(50);
        //    });

        //    modelBuilder.Entity<Record>(entity =>
        //    {
        //        entity.HasKey(e => e.RecordId);

        //        entity.ToTable("Records", "dict");

        //        entity.HasIndex(e => new { e.CourseId, e.Original })
        //            .HasName("NonClusteredIndex_Unique_Record")
        //            .IsUnique();

        //        entity.Property(e => e.Comment).HasMaxLength(200);

        //        entity.Property(e => e.Original)
        //            .IsRequired()
        //            .HasMaxLength(200);

        //        entity.Property(e => e.Translation).HasMaxLength(200);

        //        entity.HasOne(d => d.Course)
        //            .WithMany(p => p.Records)
        //            .HasForeignKey(d => d.CourseId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Records_Courses");
        //    });

        //    modelBuilder.Entity<Test>(entity =>
        //    {
        //        entity.HasKey(e => e.TestId);

        //        entity.ToTable("Tests", "dict");

        //        entity.Property(e => e.StartDate).HasColumnType("datetime");

        //        entity.Property(e => e.UserId)
        //            .IsRequired()
        //            .HasMaxLength(128);

        //        entity.HasOne(d => d.Course)
        //            .WithMany(p => p.Tests)
        //            .HasForeignKey(d => d.CourseId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Tests_Courses");

        //        entity.HasOne(d => d.PreviousTest)
        //            .WithMany(p => p.InversePreviousTest)
        //            .HasForeignKey(d => d.PreviousTestId)
        //            .HasConstraintName("FK_Tests_Tests");

        //        entity.HasOne(d => d.User)
        //            .WithMany(p => p.Tests)
        //            .HasForeignKey(d => d.UserId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_Tests_AspNetUsers");
        //    });

        //    modelBuilder.Entity<UnitTypes>(entity =>
        //    {
        //        entity.HasKey(e => e.UnitTypeId)
        //            .HasName("PK__UnitType__1B7AB934DE5079A3");

        //        entity.ToTable("UnitTypes", "cam");

        //        entity.Property(e => e.ConversionRate).HasColumnType("decimal(18, 2)");

        //        entity.Property(e => e.Description).HasMaxLength(20);

        //        entity.Property(e => e.Name)
        //            .IsRequired()
        //            .HasMaxLength(10);
        //    });
        //}
    }
}
