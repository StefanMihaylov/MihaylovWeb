using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mihaylov.Api.Other.Database.Migrations.MihaylovOtherClusterDb
{
    /// <inheritdoc />
    public partial class SettingsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                schema: "cluster",
                table: "ParserSettings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "cluster",
                table: "ParserSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParserSettingId",
                schema: "cluster",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ParserSettingId",
                schema: "cluster",
                table: "Applications",
                column: "ParserSettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ParserSettings_ParserSettingId",
                schema: "cluster",
                table: "Applications",
                column: "ParserSettingId",
                principalSchema: "cluster",
                principalTable: "ParserSettings",
                principalColumn: "ParserSettingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ParserSettings_ParserSettingId",
                schema: "cluster",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ParserSettingId",
                schema: "cluster",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "cluster",
                table: "ParserSettings");

            migrationBuilder.DropColumn(
                name: "ParserSettingId",
                schema: "cluster",
                table: "Applications");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                schema: "cluster",
                table: "ParserSettings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
