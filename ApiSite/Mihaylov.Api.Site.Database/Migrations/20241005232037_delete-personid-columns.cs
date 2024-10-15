using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Site.Database.Migrations
{
    /// <inheritdoc />
    public partial class deletepersonidcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonDetails_Persons_PersonId",
                table: "PersonDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonLocations_Persons_PersonId",
                table: "PersonLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonLocations_PersonId",
                table: "PersonLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonDetails_PersonId",
                table: "PersonDetails");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "PersonLocations");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "PersonDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PersonId",
                table: "PersonLocations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1000, 1");

            migrationBuilder.AddColumn<long>(
                name: "PersonId",
                table: "PersonDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1000, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonLocations_PersonId",
                table: "PersonLocations",
                column: "PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonDetails_PersonId",
                table: "PersonDetails",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonDetails_Persons_PersonId",
                table: "PersonDetails",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonLocations_Persons_PersonId",
                table: "PersonLocations",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "PersonId");
        }
    }
}
