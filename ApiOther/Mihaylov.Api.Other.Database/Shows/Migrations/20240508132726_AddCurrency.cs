using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mihaylov.Api.Other.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "CurrencyId",
                schema: "show",
                table: "Concerts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "show",
                columns: table => new
                {
                    CurrencyId = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CurrencyId", x => x.CurrencyId);
                });

            migrationBuilder.InsertData(
                schema: "show",
                table: "Currencies",
                columns: new[] { "CurrencyId", "Name" },
                values: new object[,]
                {
                    { (byte)1, "BGN" },
                    { (byte)2, "EUR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_CurrencyId",
                schema: "show",
                table: "Concerts",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Concerts_Currencies_CurrencyId",
                schema: "show",
                table: "Concerts",
                column: "CurrencyId",
                principalSchema: "show",
                principalTable: "Currencies",
                principalColumn: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concerts_Currencies_CurrencyId",
                schema: "show",
                table: "Concerts");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "show");

            migrationBuilder.DropIndex(
                name: "IX_Concerts_CurrencyId",
                schema: "show",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                schema: "show",
                table: "Concerts");
        }
    }
}
