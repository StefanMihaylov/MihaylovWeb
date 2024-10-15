using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Site.Database.Migrations
{
    /// <inheritdoc />
    public partial class etnicityothernames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherNames",
                table: "Ethnicities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherNames",
                table: "Ethnicities");
        }
    }
}
