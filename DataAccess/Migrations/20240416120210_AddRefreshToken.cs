using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    replaced_by_token = table.Column<string>(type: "text", nullable: true),
                    reason_revoked = table.Column<string>(type: "text", nullable: true)
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
                column: "birth_day",
                value: new DateOnly(1999, 4, 16));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -1L,
                column: "birth_day",
                value: new DateOnly(2004, 4, 16));

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 12, 14, 29, 14, 856, DateTimeKind.Unspecified).AddTicks(102), new TimeSpan(0, 3, 0, 0, 0)));

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
        }
    }
}
