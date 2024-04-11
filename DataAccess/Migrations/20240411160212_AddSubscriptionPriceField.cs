using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionPriceField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "subscriptions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "currencies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currencies", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 11, 19, 12, 11, 868, DateTimeKind.Unspecified).AddTicks(6030), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "currencies",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "RUB" },
                    { 2, "USD" }
                });

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
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 1,
                column: "price",
                value: 300m);

            migrationBuilder.UpdateData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 2,
                column: "price",
                value: 350m);

            migrationBuilder.UpdateData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 3,
                column: "price",
                value: 228m);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "currencies");

            migrationBuilder.DropColumn(
                name: "price",
                table: "subscriptions");

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
    }
}
