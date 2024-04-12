using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "comment_notification_id",
                table: "comments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "comment_notifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    readed = table.Column<bool>(type: "boolean", nullable: false),
                    comment_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comment_notifications", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                columns: new[] { "comment_notification_id", "written_at" },
                values: new object[] { null, new DateTimeOffset(new DateTime(2024, 4, 12, 14, 29, 14, 856, DateTimeKind.Unspecified).AddTicks(102), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "favourite_contents",
                keyColumns: new[] { "content_id", "user_id" },
                keyValues: new object[] { -1L, -1L },
                column: "added_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 12, 13, 49, 14, 856, DateTimeKind.Unspecified).AddTicks(92), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "reviews",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 12, 14, 19, 14, 856, DateTimeKind.Unspecified).AddTicks(98), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "user_subscriptions",
                keyColumns: new[] { "subscription_id", "user_id" },
                keyValues: new object[] { 1, -1L },
                columns: new[] { "bought_at", "expires_at" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 4, 10, 14, 19, 14, 855, DateTimeKind.Unspecified).AddTicks(9933), new TimeSpan(0, 3, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 5, 12, 14, 19, 14, 855, DateTimeKind.Unspecified).AddTicks(9967), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -2L,
                column: "birth_day",
                value: new DateOnly(1999, 4, 12));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -1L,
                column: "birth_day",
                value: new DateOnly(2004, 4, 12));

            migrationBuilder.CreateIndex(
                name: "ix_comments_comment_notification_id",
                table: "comments",
                column: "comment_notification_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_comments_comment_notifications_comment_notification_id",
                table: "comments",
                column: "comment_notification_id",
                principalTable: "comment_notifications",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_comments_comment_notifications_comment_notification_id",
                table: "comments");

            migrationBuilder.DropTable(
                name: "comment_notifications");

            migrationBuilder.DropIndex(
                name: "ix_comments_comment_notification_id",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "comment_notification_id",
                table: "comments");

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 11, 19, 12, 11, 868, DateTimeKind.Unspecified).AddTicks(6030), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "favourite_contents",
                keyColumns: new[] { "content_id", "user_id" },
                keyValues: new object[] { -1L, -1L },
                column: "added_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 11, 18, 32, 11, 868, DateTimeKind.Unspecified).AddTicks(6021), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "reviews",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 11, 19, 2, 11, 868, DateTimeKind.Unspecified).AddTicks(6026), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "user_subscriptions",
                keyColumns: new[] { "subscription_id", "user_id" },
                keyValues: new object[] { 1, -1L },
                columns: new[] { "bought_at", "expires_at" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 4, 9, 19, 2, 11, 868, DateTimeKind.Unspecified).AddTicks(5834), new TimeSpan(0, 3, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 5, 11, 19, 2, 11, 868, DateTimeKind.Unspecified).AddTicks(5861), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -2L,
                column: "birth_day",
                value: new DateOnly(1999, 4, 11));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -1L,
                column: "birth_day",
                value: new DateOnly(2004, 4, 11));
        }
    }
}
