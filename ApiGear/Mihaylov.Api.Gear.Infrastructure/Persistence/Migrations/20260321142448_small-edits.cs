using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Gear.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class smalledits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusChangedDate",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "WarrantyUntil",
                table: "InventoryItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "GearNodes",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "ItemStatuses",
                columns: new[] { "ItemStatusId", "Name" },
                values: new object[] { 5, "Ignored" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemStatuses",
                keyColumn: "ItemStatusId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "GearNodes");

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangedDate",
                table: "InventoryItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WarrantyUntil",
                table: "InventoryItems",
                type: "date",
                nullable: true);
        }
    }
}
