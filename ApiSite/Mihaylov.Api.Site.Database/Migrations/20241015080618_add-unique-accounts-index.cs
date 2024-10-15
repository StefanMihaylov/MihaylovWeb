using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Site.Database.Migrations
{
    /// <inheritdoc />
    public partial class adduniqueaccountsindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeId_Username",
                table: "Accounts",
                columns: new[] { "AccountTypeId", "Username" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_AccountTypeId_Username",
                table: "Accounts");
        }
    }
}
