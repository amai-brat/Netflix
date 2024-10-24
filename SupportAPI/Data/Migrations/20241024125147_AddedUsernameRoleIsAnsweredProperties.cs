using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsernameRoleIsAnsweredProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_answered",
                table: "support_chat_sessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "user_name",
                table: "support_chat_sessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "support_chat_messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sender_name",
                table: "support_chat_messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_answered",
                table: "support_chat_sessions");

            migrationBuilder.DropColumn(
                name: "user_name",
                table: "support_chat_sessions");

            migrationBuilder.DropColumn(
                name: "role",
                table: "support_chat_messages");

            migrationBuilder.DropColumn(
                name: "sender_name",
                table: "support_chat_messages");
        }
    }
}
