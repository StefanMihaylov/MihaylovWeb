using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mihaylov.Api.Site.Database.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountStates",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatus_StatusId", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    AccountTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes_AccountTypeId", x => x.AccountTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TwoLetterCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ThreeLetterCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    AlternativeNames = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries_CountryId", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "DateOfBirthTypes",
                columns: table => new
                {
                    DateOfBirthId = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateOfBirths_DateOfBirthId", x => x.DateOfBirthId);
                });

            migrationBuilder.CreateTable(
                name: "Ethnicities",
                columns: table => new
                {
                    EthnicityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ethnicities_EthnicityId", x => x.EthnicityId);
                });

            migrationBuilder.CreateTable(
                name: "HalfTypes",
                columns: table => new
                {
                    HalfTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HalfTypes_HalfTypeId", x => x.HalfTypeId);
                });

            migrationBuilder.CreateTable(
                name: "MediaFileExtensions",
                columns: table => new
                {
                    ExtensionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsImage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFileExtensions_ExtensionId", x => x.ExtensionId);
                });

            migrationBuilder.CreateTable(
                name: "MediaFileSources",
                columns: table => new
                {
                    SourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFileSource_SourceId", x => x.SourceId);
                });

            migrationBuilder.CreateTable(
                name: "Orientations",
                columns: table => new
                {
                    OrientationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orientations_OrientationId", x => x.OrientationId);
                });

            migrationBuilder.CreateTable(
                name: "QuizPhrases",
                columns: table => new
                {
                    PhraseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizPhrases_PhraseId", x => x.PhraseId);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions_QuestionId", x => x.QuestionId);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ConversionRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BaseUnitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units_UnitId", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_Units_Units_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "CountryStates",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryState_StateId", x => x.StateId);
                    table.ForeignKey(
                        name: "FK_CountryStates_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    DateOfBirth = table.Column<DateTime>(type: "Date", nullable: true),
                    DateOfBirthId = table.Column<byte>(type: "tinyint", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    EthnicityId = table.Column<int>(type: "int", nullable: true),
                    OrientationId = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons_PersonId", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_Persons_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                    table.ForeignKey(
                        name: "FK_Persons_DateOfBirthTypes_DateOfBirthId",
                        column: x => x.DateOfBirthId,
                        principalTable: "DateOfBirthTypes",
                        principalColumn: "DateOfBirthId");
                    table.ForeignKey(
                        name: "FK_Persons_Ethnicities_EthnicityId",
                        column: x => x.EthnicityId,
                        principalTable: "Ethnicities",
                        principalColumn: "EthnicityId");
                    table.ForeignKey(
                        name: "FK_Persons_Orientations_OrientationId",
                        column: x => x.OrientationId,
                        principalTable: "Orientations",
                        principalColumn: "OrientationId");
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    AccountTypeId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastOnlineDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts_AccountId", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountStates_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AccountStates",
                        principalColumn: "StatusId");
                    table.ForeignKey(
                        name: "FK_Accounts_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "AccountTypeId");
                    table.ForeignKey(
                        name: "FK_Accounts_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "PersonDetails",
                columns: table => new
                {
                    PersonId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OtherNames = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonDetails_PersonId", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_PersonDetails_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "PersonLocations",
                columns: table => new
                {
                    PersonId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    CountryStateId = table.Column<int>(type: "int", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonLocations_PersonId", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_PersonLocations_CountryStates_CountryStateId",
                        column: x => x.CountryStateId,
                        principalTable: "CountryStates",
                        principalColumn: "StateId");
                    table.ForeignKey(
                        name: "FK_PersonLocations_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "QuizAnswers",
                columns: table => new
                {
                    QuizAnswerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    AskDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    HalfTypeId = table.Column<int>(type: "int", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAnswers_QuizAnswerId", x => x.QuizAnswerId);
                    table.ForeignKey(
                        name: "FK_QuizAnswers_HalfTypes_HalfTypeId",
                        column: x => x.HalfTypeId,
                        principalTable: "HalfTypes",
                        principalColumn: "HalfTypeId");
                    table.ForeignKey(
                        name: "FK_QuizAnswers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_QuizAnswers_QuizQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuizQuestions",
                        principalColumn: "QuestionId");
                    table.ForeignKey(
                        name: "FK_QuizAnswers_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    MediaFileId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExtensionId = table.Column<int>(type: "int", nullable: false),
                    SizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckSum = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles_MediaFileId", x => x.MediaFileId);
                    table.ForeignKey(
                        name: "FK_MediaFiles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_MediaFileExtensions_ExtensionId",
                        column: x => x.ExtensionId,
                        principalTable: "MediaFileExtensions",
                        principalColumn: "ExtensionId");
                    table.ForeignKey(
                        name: "FK_MediaFiles_MediaFileSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "MediaFileSources",
                        principalColumn: "SourceId");
                });

            migrationBuilder.InsertData(
                table: "DateOfBirthTypes",
                columns: new[] { "DateOfBirthId", "Name" },
                values: new object[,]
                {
                    { (byte)1, "Full" },
                    { (byte)2, "YearAndMonth" },
                    { (byte)3, "YearOnly" },
                    { (byte)4, "YearCalculated" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeId",
                table: "Accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PersonId",
                table: "Accounts",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_StatusId",
                table: "Accounts",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_ThreeLetterCode",
                table: "Countries",
                column: "ThreeLetterCode",
                unique: true,
                filter: "[ThreeLetterCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_TwoLetterCode",
                table: "Countries",
                column: "TwoLetterCode",
                unique: true,
                filter: "[TwoLetterCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CountryStates_CountryId",
                table: "CountryStates",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DateOfBirthTypes_Name",
                table: "DateOfBirthTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_AccountId",
                table: "MediaFiles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_CheckSum",
                table: "MediaFiles",
                column: "CheckSum",
                unique: true,
                filter: "[CheckSum] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_ExtensionId",
                table: "MediaFiles",
                column: "ExtensionId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_GroupId",
                table: "MediaFiles",
                column: "GroupId",
                filter: "[GroupId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_SourceId",
                table: "MediaFiles",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonLocations_CountryStateId",
                table: "PersonLocations",
                column: "CountryStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CountryId",
                table: "Persons",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_DateOfBirthId",
                table: "Persons",
                column: "DateOfBirthId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_EthnicityId",
                table: "Persons",
                column: "EthnicityId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_OrientationId",
                table: "Persons",
                column: "OrientationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswers_HalfTypeId",
                table: "QuizAnswers",
                column: "HalfTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswers_PersonId",
                table: "QuizAnswers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswers_QuestionId",
                table: "QuizAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswers_UnitId",
                table: "QuizAnswers",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizPhrases_OrderId",
                table: "QuizPhrases",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_BaseUnitId",
                table: "Units",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Name",
                table: "Units",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "PersonDetails");

            migrationBuilder.DropTable(
                name: "PersonLocations");

            migrationBuilder.DropTable(
                name: "QuizAnswers");

            migrationBuilder.DropTable(
                name: "QuizPhrases");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "MediaFileExtensions");

            migrationBuilder.DropTable(
                name: "MediaFileSources");

            migrationBuilder.DropTable(
                name: "CountryStates");

            migrationBuilder.DropTable(
                name: "HalfTypes");

            migrationBuilder.DropTable(
                name: "QuizQuestions");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "AccountStates");

            migrationBuilder.DropTable(
                name: "AccountTypes");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "DateOfBirthTypes");

            migrationBuilder.DropTable(
                name: "Ethnicities");

            migrationBuilder.DropTable(
                name: "Orientations");
        }
    }
}
