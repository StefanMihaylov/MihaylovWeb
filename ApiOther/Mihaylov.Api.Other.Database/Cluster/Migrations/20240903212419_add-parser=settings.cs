using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mihaylov.Api.Other.Database.Migrations.MihaylovOtherClusterDb
{
    /// <inheritdoc />
    public partial class addparsersettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GithubVersionUrl",
                schema: "cluster",
                table: "Applications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteUrl",
                schema: "cluster",
                table: "Applications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VersionUrlTypes",
                schema: "cluster",
                columns: table => new
                {
                    VersionUrlId = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VersionUrlId", x => x.VersionUrlId);
                });

            migrationBuilder.CreateTable(
                name: "ParserSettings",
                schema: "cluster",
                columns: table => new
                {
                    ParserSettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    VersionUrlVersionId = table.Column<byte>(type: "tinyint", nullable: false),
                    SelectorVersion = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CommandsVersion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VersionUrlrReleaseId = table.Column<byte>(type: "tinyint", nullable: true),
                    SelectorRelease = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CommandsRelease = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ParserSettingId", x => x.ParserSettingId);
                    table.ForeignKey(
                        name: "FK_ParserSettings_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "cluster",
                        principalTable: "Applications",
                        principalColumn: "ApplicationId");
                    table.ForeignKey(
                        name: "FK_ParserSettings_VersionUrlTypes_VersionUrlVersionId",
                        column: x => x.VersionUrlVersionId,
                        principalSchema: "cluster",
                        principalTable: "VersionUrlTypes",
                        principalColumn: "VersionUrlId");
                    table.ForeignKey(
                        name: "FK_ParserSettings_VersionUrlTypes_VersionUrlrReleaseId",
                        column: x => x.VersionUrlrReleaseId,
                        principalSchema: "cluster",
                        principalTable: "VersionUrlTypes",
                        principalColumn: "VersionUrlId");
                });

            migrationBuilder.InsertData(
                schema: "cluster",
                table: "VersionUrlTypes",
                columns: new[] { "VersionUrlId", "Name" },
                values: new object[,]
                {
                    { (byte)1, "SiteUrl" },
                    { (byte)2, "ReleaseUrl" },
                    { (byte)3, "GithubVersionUrl" },
                    { (byte)4, "ResourceUrl" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParserSettings_ApplicationId",
                schema: "cluster",
                table: "ParserSettings",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ParserSettings_VersionUrlrReleaseId",
                schema: "cluster",
                table: "ParserSettings",
                column: "VersionUrlrReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ParserSettings_VersionUrlVersionId",
                schema: "cluster",
                table: "ParserSettings",
                column: "VersionUrlVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionUrlTypes_Name",
                schema: "cluster",
                table: "VersionUrlTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParserSettings",
                schema: "cluster");

            migrationBuilder.DropTable(
                name: "VersionUrlTypes",
                schema: "cluster");

            migrationBuilder.DropColumn(
                name: "GithubVersionUrl",
                schema: "cluster",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "SiteUrl",
                schema: "cluster",
                table: "Applications");
        }
    }
}
