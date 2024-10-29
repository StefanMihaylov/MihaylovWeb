using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Dictionary.Database.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages_LanguageId", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    LevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descrition = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels_LevelId", x => x.LevelId);
                });

            migrationBuilder.CreateTable(
                name: "RecordTypes",
                columns: table => new
                {
                    RecordTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordTypes_RecordTypeId", x => x.RecordTypeId);
                });

            migrationBuilder.CreateTable(
                name: "LearningSystems",
                columns: table => new
                {
                    LearningSystemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningSystems_LearningSystemId", x => x.LearningSystemId);
                    table.ForeignKey(
                        name: "FK_LearningSystems_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId");
                });

            migrationBuilder.CreateTable(
                name: "Prepositions",
                columns: table => new
                {
                    PrepositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prepositions_PrepositionTypeId", x => x.PrepositionId);
                    table.ForeignKey(
                        name: "FK_Prepositions_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LearningSystemId = table.Column<int>(type: "int", nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "Date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "Date", nullable: true),
                    ModulesStartNumber = table.Column<int>(type: "int", nullable: true),
                    ModulesEndNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses_CourseId", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_LearningSystems_LearningSystemId",
                        column: x => x.LearningSystemId,
                        principalTable: "LearningSystems",
                        principalColumn: "LearningSystemId");
                    table.ForeignKey(
                        name: "FK_Courses_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "LevelId");
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    ModuleNumber = table.Column<int>(type: "int", nullable: true),
                    RecordTypeId = table.Column<int>(type: "int", nullable: false),
                    Original = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrepositionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records_RecordId", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_Records_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_Records_Prepositions_PrepositionId",
                        column: x => x.PrepositionId,
                        principalTable: "Prepositions",
                        principalColumn: "PrepositionId");
                    table.ForeignKey(
                        name: "FK_Records_RecordTypes_RecordTypeId",
                        column: x => x.RecordTypeId,
                        principalTable: "RecordTypes",
                        principalColumn: "RecordTypeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LearningSystemId",
                table: "Courses",
                column: "LearningSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LevelId",
                table: "Courses",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningSystems_LanguageId",
                table: "LearningSystems",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Prepositions_LanguageId",
                table: "Prepositions",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_CourseId_Original",
                table: "Records",
                columns: new[] { "CourseId", "Original" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_PrepositionId",
                table: "Records",
                column: "PrepositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_RecordTypeId",
                table: "Records",
                column: "RecordTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Prepositions");

            migrationBuilder.DropTable(
                name: "RecordTypes");

            migrationBuilder.DropTable(
                name: "LearningSystems");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
