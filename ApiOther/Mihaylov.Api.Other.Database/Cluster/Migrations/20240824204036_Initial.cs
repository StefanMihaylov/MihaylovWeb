using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mihaylov.Api.Other.Database.Migrations.MihaylovOtherClusterDb
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cluster");

            migrationBuilder.CreateTable(
                name: "DeploymentTypes",
                schema: "cluster",
                columns: table => new
                {
                    DeploymentId = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DeploymentId", x => x.DeploymentId);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                schema: "cluster",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReleaseUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ResourceUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeploymentId = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ApplicationId", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_Applications_DeploymentTypes_DeploymentId",
                        column: x => x.DeploymentId,
                        principalSchema: "cluster",
                        principalTable: "DeploymentTypes",
                        principalColumn: "DeploymentId");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationPods",
                schema: "cluster",
                columns: table => new
                {
                    ApplicationPodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ApplicationPodId", x => x.ApplicationPodId);
                    table.ForeignKey(
                        name: "FK_ApplicationPods_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "cluster",
                        principalTable: "Applications",
                        principalColumn: "ApplicationId");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationVersions",
                schema: "cluster",
                columns: table => new
                {
                    VersionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HelmVersion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HelmAppVersion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "Date", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VersionId", x => x.VersionId);
                    table.ForeignKey(
                        name: "FK_ApplicationVersions_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "cluster",
                        principalTable: "Applications",
                        principalColumn: "ApplicationId");
                });

            migrationBuilder.CreateTable(
                name: "DeploymentFiles",
                schema: "cluster",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FileId", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_DeploymentFiles_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "cluster",
                        principalTable: "Applications",
                        principalColumn: "ApplicationId");
                });

            migrationBuilder.InsertData(
                schema: "cluster",
                table: "DeploymentTypes",
                columns: new[] { "DeploymentId", "Name" },
                values: new object[,]
                {
                    { (byte)1, "Yaml" },
                    { (byte)2, "HelmChart" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationPods_ApplicationId",
                schema: "cluster",
                table: "ApplicationPods",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationPods_Name",
                schema: "cluster",
                table: "ApplicationPods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_DeploymentId",
                schema: "cluster",
                table: "Applications",
                column: "DeploymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Name",
                schema: "cluster",
                table: "Applications",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationVersions_ApplicationId",
                schema: "cluster",
                table: "ApplicationVersions",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentFiles_ApplicationId",
                schema: "cluster",
                table: "DeploymentFiles",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentTypes_Name",
                schema: "cluster",
                table: "DeploymentTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationPods",
                schema: "cluster");

            migrationBuilder.DropTable(
                name: "ApplicationVersions",
                schema: "cluster");

            migrationBuilder.DropTable(
                name: "DeploymentFiles",
                schema: "cluster");

            migrationBuilder.DropTable(
                name: "Applications",
                schema: "cluster");

            migrationBuilder.DropTable(
                name: "DeploymentTypes",
                schema: "cluster");
        }
    }
}
