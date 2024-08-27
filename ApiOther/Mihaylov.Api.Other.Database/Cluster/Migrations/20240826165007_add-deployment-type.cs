using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Other.Database.Migrations.MihaylovOtherClusterDb
{
    /// <inheritdoc />
    public partial class adddeploymenttype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "cluster",
                table: "DeploymentTypes",
                columns: new[] { "DeploymentId", "Name" },
                values: new object[] { (byte)3, "Command" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "cluster",
                table: "DeploymentTypes",
                keyColumn: "DeploymentId",
                keyValue: (byte)3);
        }
    }
}
