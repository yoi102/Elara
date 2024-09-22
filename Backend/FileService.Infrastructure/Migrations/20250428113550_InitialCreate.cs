using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_UploadedItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    FileSizeInBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    FileType = table.Column<string>(type: "TEXT", nullable: false),
                    FileSHA256Hash = table.Column<string>(type: "TEXT", unicode: false, maxLength: 64, nullable: false),
                    RemoteUrl = table.Column<string>(type: "TEXT", nullable: false),
                    BackupUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UploadedItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_UploadedItems_FileSHA256Hash_FileSizeInBytes",
                table: "T_UploadedItems",
                columns: new[] { "FileSHA256Hash", "FileSizeInBytes" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_UploadedItems");
        }
    }
}
