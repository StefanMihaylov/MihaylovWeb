using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Site.Database.Migrations
{
    /// <inheritdoc />
    public partial class datetimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AskDate",
                table: "QuizAnswers",
                type: "datetime2(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "MediaFiles",
                type: "datetime2(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastOnlineDate",
                table: "Accounts",
                type: "datetime2(3)",
                precision: 3,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AskDate",
                table: "QuizAnswers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(3)",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "MediaFiles",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(3)",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastOnlineDate",
                table: "Accounts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(3)",
                oldPrecision: 3,
                oldNullable: true);
        }
    }
}
