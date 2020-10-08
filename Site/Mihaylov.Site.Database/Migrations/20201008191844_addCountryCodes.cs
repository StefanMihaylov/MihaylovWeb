using Microsoft.EntityFrameworkCore.Migrations;

namespace Mihaylov.Site.Database.Migrations
{
    public partial class addCountryCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateCode",
                table: "States",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Countries",
                maxLength: 2,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateCode",
                table: "States");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Countries");
        }
    }
}
