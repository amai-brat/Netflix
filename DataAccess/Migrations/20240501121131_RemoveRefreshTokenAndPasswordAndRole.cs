using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRefreshTokenAndPasswordAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "password",
                table: "users");

            migrationBuilder.DropColumn(
                name: "role",
                table: "users");

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 5, 1, 15, 21, 31, 5, DateTimeKind.Unspecified).AddTicks(8544), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "favourite_contents",
                keyColumns: new[] { "content_id", "user_id" },
                keyValues: new object[] { -1L, -1L },
                column: "added_at",
                value: new DateTimeOffset(new DateTime(2024, 5, 1, 14, 41, 31, 5, DateTimeKind.Unspecified).AddTicks(8534), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "reviews",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 5, 1, 15, 11, 31, 5, DateTimeKind.Unspecified).AddTicks(8540), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "user_subscriptions",
                keyColumns: new[] { "subscription_id", "user_id" },
                keyValues: new object[] { 1, -1L },
                columns: new[] { "bought_at", "expires_at" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 4, 29, 15, 11, 31, 5, DateTimeKind.Unspecified).AddTicks(8349), new TimeSpan(0, 3, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 5, 31, 15, 11, 31, 5, DateTimeKind.Unspecified).AddTicks(8373), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -2L,
                column: "birth_day",
                value: new DateOnly(1999, 5, 1));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -1L,
                column: "birth_day",
                value: new DateOnly(2004, 5, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "user");

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    reason_revoked = table.Column<string>(type: "text", nullable: true),
                    replaced_by_token = table.Column<string>(type: "text", nullable: true),
                    revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    token = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 16, 15, 12, 9, 931, DateTimeKind.Unspecified).AddTicks(8855), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "favourite_contents",
                keyColumns: new[] { "content_id", "user_id" },
                keyValues: new object[] { -1L, -1L },
                column: "added_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 16, 14, 32, 9, 931, DateTimeKind.Unspecified).AddTicks(8845), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "reviews",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 16, 15, 2, 9, 931, DateTimeKind.Unspecified).AddTicks(8850), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "user_subscriptions",
                keyColumns: new[] { "subscription_id", "user_id" },
                keyValues: new object[] { 1, -1L },
                columns: new[] { "bought_at", "expires_at" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 4, 14, 15, 2, 9, 931, DateTimeKind.Unspecified).AddTicks(8658), new TimeSpan(0, 3, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 5, 16, 15, 2, 9, 931, DateTimeKind.Unspecified).AddTicks(8685), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -2L,
                columns: new[] { "birth_day", "password", "role" },
                values: new object[] { new DateOnly(1999, 4, 16), "testPassword1337;", "user" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -1L,
                columns: new[] { "birth_day", "password", "role" },
                values: new object[] { new DateOnly(2004, 4, 16), "testPassword228;", "user" });

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");
        }
    }
}
