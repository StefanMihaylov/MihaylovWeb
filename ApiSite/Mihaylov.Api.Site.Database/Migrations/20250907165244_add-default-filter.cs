using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Site.Database.Migrations
{
    /// <inheritdoc />
    public partial class adddefaultfilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefaultFilters",
                columns: table => new
                {
                    DefaultFilterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AccountTypeId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    IsArchive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPreview = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultFilters_DefaultFilterId", x => x.DefaultFilterId);
                    table.ForeignKey(
                        name: "FK_DefaultFilters_AccountStates_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AccountStates",
                        principalColumn: "StatusId");
                    table.ForeignKey(
                        name: "FK_DefaultFilters_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "AccountTypeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefaultFilters_AccountTypeId",
                table: "DefaultFilters",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultFilters_StatusId",
                table: "DefaultFilters",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultFilters");
        }
    }
}
