using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Other.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "show");

            migrationBuilder.CreateTable(
                name: "Bands",
                schema: "show",
                columns: table => new
                {
                    BandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("BandId", x => x.BandId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "show",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LocationId", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "TicketProviders",
                schema: "show",
                columns: table => new
                {
                    TickerProviderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TickerProviderId", x => x.TickerProviderId);
                });

            migrationBuilder.CreateTable(
                name: "Concerts",
                schema: "show",
                columns: table => new
                {
                    ConcertId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TicketProviderId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ConcertId", x => x.ConcertId);
                    table.ForeignKey(
                        name: "FK_Concerts_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "show",
                        principalTable: "Locations",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "FK_Concerts_TicketProviders_TicketProviderId",
                        column: x => x.TicketProviderId,
                        principalSchema: "show",
                        principalTable: "TicketProviders",
                        principalColumn: "TickerProviderId");
                });

            migrationBuilder.CreateTable(
                name: "ConcertBands",
                schema: "show",
                columns: table => new
                {
                    BandId = table.Column<int>(type: "int", nullable: false),
                    ConcertId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcertBands", x => new { x.BandId, x.ConcertId });
                    table.ForeignKey(
                        name: "FK_ConcertBands_Bands_BandId",
                        column: x => x.BandId,
                        principalSchema: "show",
                        principalTable: "Bands",
                        principalColumn: "BandId");
                    table.ForeignKey(
                        name: "FK_ConcertBands_Concerts_ConcertId",
                        column: x => x.ConcertId,
                        principalSchema: "show",
                        principalTable: "Concerts",
                        principalColumn: "ConcertId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConcertBands_ConcertId",
                schema: "show",
                table: "ConcertBands",
                column: "ConcertId");

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_LocationId",
                schema: "show",
                table: "Concerts",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_TicketProviderId",
                schema: "show",
                table: "Concerts",
                column: "TicketProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConcertBands",
                schema: "show");

            migrationBuilder.DropTable(
                name: "Bands",
                schema: "show");

            migrationBuilder.DropTable(
                name: "Concerts",
                schema: "show");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "show");

            migrationBuilder.DropTable(
                name: "TicketProviders",
                schema: "show");
        }
    }
}
