using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mihaylov.Site.Database.Migrations
{
    public partial class addMediafiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    SourceId = table.Column<int>(nullable: false),
                    Data = table.Column<byte[]>(nullable: false),
                    Extension = table.Column<string>(maxLength: 10, nullable: false),
                    SizeInBytes = table.Column<long>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaFiles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediaFiles_AccountTypes_SourceId",
                        column: x => x.SourceId,
                        principalTable: "AccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_AccountId",
                table: "MediaFiles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_SourceId",
                table: "MediaFiles",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaFiles");
        }
    }
}
