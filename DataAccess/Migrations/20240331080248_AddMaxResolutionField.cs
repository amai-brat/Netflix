using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMaxResolutionField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "max_resolution",
                table: "subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 3, 31, 11, 12, 47, 690, DateTimeKind.Unspecified).AddTicks(3130), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "favourite_contents",
                keyColumns: new[] { "content_id", "user_id" },
                keyValues: new object[] { -1L, -1L },
                column: "added_at",
                value: new DateTimeOffset(new DateTime(2024, 3, 31, 10, 32, 47, 690, DateTimeKind.Unspecified).AddTicks(3120), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "reviews",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 3, 31, 11, 2, 47, 690, DateTimeKind.Unspecified).AddTicks(3126), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 1,
                column: "max_resolution",
                value: 2160);

            migrationBuilder.UpdateData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 2,
                column: "max_resolution",
                value: 1080);

            migrationBuilder.UpdateData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 3,
                column: "max_resolution",
                value: 720);

            migrationBuilder.UpdateData(
                table: "user_subscriptions",
                keyColumns: new[] { "subscription_id", "user_id" },
                keyValues: new object[] { 1, -1L },
                columns: new[] { "bought_at", "expires_at" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 3, 29, 11, 2, 47, 690, DateTimeKind.Unspecified).AddTicks(2961), new TimeSpan(0, 3, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 4, 30, 11, 2, 47, 690, DateTimeKind.Unspecified).AddTicks(2998), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -2L,
                column: "birth_day",
                value: new DateOnly(1999, 3, 31));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -1L,
                column: "birth_day",
                value: new DateOnly(2004, 3, 31));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "max_resolution",
                table: "subscriptions");

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 3, 25, 12, 51, 22, 134, DateTimeKind.Unspecified).AddTicks(9994), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "favourite_contents",
                keyColumns: new[] { "content_id", "user_id" },
                keyValues: new object[] { -1L, -1L },
                column: "added_at",
                value: new DateTimeOffset(new DateTime(2024, 3, 25, 12, 11, 22, 134, DateTimeKind.Unspecified).AddTicks(9973), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "reviews",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 3, 25, 12, 41, 22, 134, DateTimeKind.Unspecified).AddTicks(9984), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "user_subscriptions",
                keyColumns: new[] { "subscription_id", "user_id" },
                keyValues: new object[] { 1, -1L },
                columns: new[] { "bought_at", "expires_at" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 3, 23, 12, 41, 22, 134, DateTimeKind.Unspecified).AddTicks(9768), new TimeSpan(0, 3, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 4, 24, 12, 41, 22, 134, DateTimeKind.Unspecified).AddTicks(9806), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -2L,
                column: "birth_day",
                value: new DateOnly(1999, 3, 25));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -1L,
                column: "birth_day",
                value: new DateOnly(2004, 3, 25));
        }
    }
}
