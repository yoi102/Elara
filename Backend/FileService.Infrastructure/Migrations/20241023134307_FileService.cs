using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileService.Infrastructure.Migrations;

/// <inheritdoc />
public partial class FileService : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "FileService_UploadedItems",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CreationTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                FileSizeInBytes = table.Column<long>(type: "INTEGER", nullable: false),
                FileName = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                FileSHA256Hash = table.Column<string>(type: "TEXT", unicode: false, maxLength: 64, nullable: false),
                RemoteUrl = table.Column<string>(type: "TEXT", nullable: false),
                BackupUrl = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FileService_UploadedItems", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_FileService_UploadedItems_FileSHA256Hash_FileSizeInBytes",
            table: "FileService_UploadedItems",
            columns: new[] { "FileSHA256Hash", "FileSizeInBytes" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FileService_UploadedItems");
    }
}
