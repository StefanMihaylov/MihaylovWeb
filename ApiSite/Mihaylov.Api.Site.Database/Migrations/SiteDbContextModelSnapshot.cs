﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mihaylov.Api.Site.Database;

#nullable disable

namespace Mihaylov.Api.Site.Database.Migrations
{
    [DbContext(typeof(SiteDbContext))]
    partial class SiteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Account", b =>
                {
                    b.Property<long>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("AccountId"), 1000L);

                    b.Property<int>("AccountTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("Date");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("CreatedOn")
                        .HasPrecision(3)
                        .HasColumnType("datetime2(3)");

                    b.Property<string>("Details")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LastOnlineDate")
                        .HasPrecision(3)
                        .HasColumnType("datetime2(3)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasPrecision(3)
                        .HasColumnType("datetime2(3)");

                    b.Property<long?>("PersonId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ReconciledDate")
                        .HasColumnType("Date");

                    b.Property<int?>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AccountId")
                        .HasName("PK_Accounts_AccountId");

                    b.HasIndex("AccountTypeId");

                    b.HasIndex("PersonId");

                    b.HasIndex("StatusId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.AccountStatus", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StatusId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("StatusId")
                        .HasName("PK_AccountStatus_StatusId");

                    b.ToTable("AccountStates");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.AccountType", b =>
                {
                    b.Property<int>("AccountTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountTypeId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AccountTypeId")
                        .HasName("PK_AccountTypes_AccountTypeId");

                    b.ToTable("AccountTypes");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"));

                    b.Property<string>("AlternativeNames")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ThreeLetterCode")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("TwoLetterCode")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.HasKey("CountryId")
                        .HasName("PK_Countries_CountryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ThreeLetterCode")
                        .IsUnique()
                        .HasFilter("[ThreeLetterCode] IS NOT NULL");

                    b.HasIndex("TwoLetterCode")
                        .IsUnique()
                        .HasFilter("[TwoLetterCode] IS NOT NULL");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.CountryState", b =>
                {
                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StateId"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("StateId")
                        .HasName("PK_CountryState_StateId");

                    b.HasIndex("CountryId");

                    b.ToTable("CountryStates");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.DateOfBirth", b =>
                {
                    b.Property<byte>("DateOfBirthId")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("DateOfBirthId")
                        .HasName("PK_DateOfBirths_DateOfBirthId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("DateOfBirthTypes");

                    b.HasData(
                        new
                        {
                            DateOfBirthId = (byte)1,
                            Name = "Full"
                        },
                        new
                        {
                            DateOfBirthId = (byte)2,
                            Name = "YearAndMonth"
                        },
                        new
                        {
                            DateOfBirthId = (byte)3,
                            Name = "YearOnly"
                        },
                        new
                        {
                            DateOfBirthId = (byte)4,
                            Name = "YearCalculated"
                        });
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Ethnicity", b =>
                {
                    b.Property<int>("EthnicityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EthnicityId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("EthnicityId")
                        .HasName("PK_Ethnicities_EthnicityId");

                    b.ToTable("Ethnicities");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.HalfType", b =>
                {
                    b.Property<int>("HalfTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HalfTypeId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("HalfTypeId")
                        .HasName("PK_HalfTypes_HalfTypeId");

                    b.ToTable("HalfTypes");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.MediaFile", b =>
                {
                    b.Property<long>("MediaFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("MediaFileId"));

                    b.Property<long?>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("CheckSum")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreateDate")
                        .HasPrecision(3)
                        .HasColumnType("datetime2(3)");

                    b.Property<int>("ExtensionId")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<long>("SizeInBytes")
                        .HasColumnType("bigint");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.HasKey("MediaFileId")
                        .HasName("PK_MediaFiles_MediaFileId");

                    b.HasIndex("AccountId");

                    b.HasIndex("CheckSum")
                        .IsUnique()
                        .HasFilter("[CheckSum] IS NOT NULL");

                    b.HasIndex("ExtensionId");

                    b.HasIndex("GroupId")
                        .HasFilter("[GroupId] IS NOT NULL");

                    b.HasIndex("SourceId");

                    b.ToTable("MediaFiles");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.MediaFileExtension", b =>
                {
                    b.Property<int>("ExtensionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExtensionId"));

                    b.Property<bool>("IsImage")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("ExtensionId")
                        .HasName("PK_MediaFileExtensions_ExtensionId");

                    b.ToTable("MediaFileExtensions");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.MediaFileSource", b =>
                {
                    b.Property<int>("SourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SourceId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("SourceId")
                        .HasName("PK_MediaFileSource_SourceId");

                    b.ToTable("MediaFileSources");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Orientation", b =>
                {
                    b.Property<int>("OrientationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrientationId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("OrientationId")
                        .HasName("PK_Orientations_OrientationId");

                    b.ToTable("Orientations");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Person", b =>
                {
                    b.Property<long>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("PersonId"), 1000L);

                    b.Property<string>("Comments")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("CreatedOn")
                        .HasPrecision(3)
                        .HasColumnType("datetime2(3)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("Date");

                    b.Property<byte?>("DateOfBirthId")
                        .HasColumnType("tinyint");

                    b.Property<int?>("EthnicityId")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasPrecision(3)
                        .HasColumnType("datetime2(3)");

                    b.Property<int?>("OrientationId")
                        .HasColumnType("int");

                    b.HasKey("PersonId")
                        .HasName("PK_Persons_PersonId");

                    b.HasIndex("CountryId");

                    b.HasIndex("DateOfBirthId");

                    b.HasIndex("EthnicityId");

                    b.HasIndex("OrientationId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.PersonDetail", b =>
                {
                    b.Property<long>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("PersonId"), 1000L);

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OtherNames")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("PersonId")
                        .HasName("PK_PersonDetails_PersonId");

                    b.ToTable("PersonDetails");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.PersonLocation", b =>
                {
                    b.Property<long>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("PersonId"), 1000L);

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("CountryStateId")
                        .HasColumnType("int");

                    b.Property<string>("Details")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Region")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("PersonId")
                        .HasName("PK_PersonLocations_PersonId");

                    b.HasIndex("CountryStateId");

                    b.ToTable("PersonLocations");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.QuizAnswer", b =>
                {
                    b.Property<long>("QuizAnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("QuizAnswerId"));

                    b.Property<DateTime>("AskDate")
                        .HasPrecision(3)
                        .HasColumnType("datetime2(3)");

                    b.Property<string>("Details")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int?>("HalfTypeId")
                        .HasColumnType("int");

                    b.Property<long>("PersonId")
                        .HasColumnType("bigint");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int?>("UnitId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Value")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("QuizAnswerId")
                        .HasName("PK_QuizAnswers_QuizAnswerId");

                    b.HasIndex("HalfTypeId");

                    b.HasIndex("PersonId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UnitId");

                    b.ToTable("QuizAnswers");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.QuizPhrase", b =>
                {
                    b.Property<int>("PhraseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhraseId"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("PhraseId")
                        .HasName("PK_QuizPhrases_PhraseId");

                    b.HasIndex("OrderId");

                    b.ToTable("QuizPhrases");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.QuizQuestion", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionId"));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("QuestionId")
                        .HasName("PK_QuizQuestions_QuestionId");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Unit", b =>
                {
                    b.Property<int>("UnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UnitId"));

                    b.Property<int?>("BaseUnitId")
                        .HasColumnType("int");

                    b.Property<decimal?>("ConversionRate")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("UnitId")
                        .HasName("PK_Units_UnitId");

                    b.HasIndex("BaseUnitId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Account", b =>
                {
                    b.HasOne("Mihaylov.Api.Site.Database.Models.AccountType", "AccountType")
                        .WithMany()
                        .HasForeignKey("AccountTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Mihaylov.Api.Site.Database.Models.Person", "Person")
                        .WithMany("Accounts")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Mihaylov.Api.Site.Database.Models.AccountStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("AccountType");

                    b.Navigation("Person");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.CountryState", b =>
                {
                    b.HasOne("Mihaylov.Api.Site.Database.Models.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.MediaFile", b =>
                {
                    b.HasOne("Mihaylov.Api.Site.Database.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Mihaylov.Api.Site.Database.Models.MediaFileExtension", "Extension")
                        .WithMany()
                        .HasForeignKey("ExtensionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Mihaylov.Api.Site.Database.Models.MediaFileSource", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Extension");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Person", b =>
                {
                    b.HasOne("Mihaylov.Api.Site.Database.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Mihaylov.Api.Site.Database.Models.DateOfBirth", "DateOfBirthType")
                        .WithMany()
                        .HasForeignKey("DateOfBirthId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Mihaylov.Api.Site.Database.Models.Ethnicity", "Ethnicity")
                        .WithMany()
                        .HasForeignKey("EthnicityId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Mihaylov.Api.Site.Database.Models.Orientation", "Orientation")
                        .WithMany()
                        .HasForeignKey("OrientationId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Country");

                    b.Navigation("DateOfBirthType");

                    b.Navigation("Ethnicity");

                    b.Navigation("Orientation");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.PersonDetail", b =>
                {
                    b.HasOne("Mihaylov.Api.Site.Database.Models.Person", "Person")
                        .WithOne("Details")
                        .HasForeignKey("Mihaylov.Api.Site.Database.Models.PersonDetail", "PersonId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.PersonLocation", b =>
                {
                    b.HasOne("Mihaylov.Api.Site.Database.Models.CountryState", "CountryState")
                        .WithMany()
                        .HasForeignKey("CountryStateId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Mihaylov.Api.Site.Database.Models.Person", "Person")
                        .WithOne("Location")
                        .HasForeignKey("Mihaylov.Api.Site.Database.Models.PersonLocation", "PersonId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("CountryState");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.QuizAnswer", b =>
                {
                    b.HasOne("Mihaylov.Api.Site.Database.Models.HalfType", "HalfType")
                        .WithMany()
                        .HasForeignKey("HalfTypeId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Mihaylov.Api.Site.Database.Models.Person", "Person")
                        .WithMany("Answers")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Mihaylov.Api.Site.Database.Models.QuizQuestion", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Mihaylov.Api.Site.Database.Models.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("HalfType");

                    b.Navigation("Person");

                    b.Navigation("Question");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Unit", b =>
                {
                    b.HasOne("Mihaylov.Api.Site.Database.Models.Unit", "BaseUnit")
                        .WithMany()
                        .HasForeignKey("BaseUnitId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("BaseUnit");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Country", b =>
                {
                    b.Navigation("States");
                });

            modelBuilder.Entity("Mihaylov.Api.Site.Database.Models.Person", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Answers");

                    b.Navigation("Details");

                    b.Navigation("Location");
                });
#pragma warning restore 612, 618
        }
    }
}