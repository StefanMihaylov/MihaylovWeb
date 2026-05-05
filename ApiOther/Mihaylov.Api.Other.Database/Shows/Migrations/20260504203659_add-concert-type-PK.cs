using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Other.Database.Migrations
{
    /// <inheritdoc />
    public partial class addconcerttypePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConcertTypeId",
                schema: "show",
                table: "ConcertTypes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "ConcertTypeId",
                schema: "show",
                table: "ConcertTypes",
                column: "ConcertTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_ConcertTypeId",
                schema: "show",
                table: "Concerts",
                column: "ConcertTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Concerts_ConcertTypes_ConcertTypeId",
                schema: "show",
                table: "Concerts",
                column: "ConcertTypeId",
                principalSchema: "show",
                principalTable: "ConcertTypes",
                principalColumn: "ConcertTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concerts_ConcertTypes_ConcertTypeId",
                schema: "show",
                table: "Concerts");

            migrationBuilder.DropPrimaryKey(
                name: "ConcertTypeId",
                schema: "show",
                table: "ConcertTypes");

            migrationBuilder.DropIndex(
                name: "IX_Concerts_ConcertTypeId",
                schema: "show",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "ConcertTypeId",
                schema: "show",
                table: "ConcertTypes");
        }
    }
}
