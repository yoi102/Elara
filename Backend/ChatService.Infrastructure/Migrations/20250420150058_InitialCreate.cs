using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_ConversationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ConversationRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsGroup = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Conversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConversationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuoteMessageId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    SenderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Messages_T_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "T_Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConversationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Participants_T_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "T_Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_MessageAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UploadedItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MessageAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_MessageAttachments_T_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "T_Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_ReplyMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RepliedMessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OriginalMessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ReplyMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_ReplyMessages_T_Messages_OriginalMessageId",
                        column: x => x.OriginalMessageId,
                        principalTable: "T_Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_ReplyMessages_T_Messages_RepliedMessageId",
                        column: x => x.RepliedMessageId,
                        principalTable: "T_Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_UserUnreadMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    HasBeenRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UserUnreadMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_UserUnreadMessages_T_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "T_Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_MessageAttachments_MessageId",
                table: "T_MessageAttachments",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Messages_ConversationId",
                table: "T_Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Participants_ConversationId_UserId",
                table: "T_Participants",
                columns: new[] { "ConversationId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_ReplyMessages_OriginalMessageId",
                table: "T_ReplyMessages",
                column: "OriginalMessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_ReplyMessages_RepliedMessageId",
                table: "T_ReplyMessages",
                column: "RepliedMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_T_UserUnreadMessages_MessageId",
                table: "T_UserUnreadMessages",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_ConversationRequests");

            migrationBuilder.DropTable(
                name: "T_MessageAttachments");

            migrationBuilder.DropTable(
                name: "T_Participants");

            migrationBuilder.DropTable(
                name: "T_ReplyMessages");

            migrationBuilder.DropTable(
                name: "T_UserUnreadMessages");

            migrationBuilder.DropTable(
                name: "T_Messages");

            migrationBuilder.DropTable(
                name: "T_Conversations");
        }
    }
}
