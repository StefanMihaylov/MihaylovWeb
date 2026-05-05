using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Other.Database.Migrations
{
    /// <inheritdoc />
    public partial class deleteconcerttypePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "ConcertTypeId",
                schema: "show",
                table: "Concerts",
                type: "int",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "ConcertTypeId",
                schema: "show",
                table: "ConcertTypes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<byte>(
                name: "ConcertTypeId",
                schema: "show",
                table: "Concerts",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
