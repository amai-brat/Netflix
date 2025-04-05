using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RefactorUserSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_subscriptions",
                table: "user_subscriptions");

            migrationBuilder.DeleteData(
                table: "user_subscriptions",
                keyColumns: new[] { "subscription_id", "user_id" },
                keyValues: new object[] { 1, -1L });

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "user_subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "user_subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "transaction_id",
                table: "user_subscriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_subscriptions",
                table: "user_subscriptions",
                column: "id");

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2025, 4, 5, 8, 50, 8, 916, DateTimeKind.Unspecified).AddTicks(2263), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "favourite_contents",
                keyColumns: new[] { "content_id", "user_id" },
                keyValues: new object[] { -1L, -1L },
                column: "added_at",
                value: new DateTimeOffset(new DateTime(2025, 4, 5, 8, 10, 8, 916, DateTimeKind.Unspecified).AddTicks(2249), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "reviews",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2025, 4, 5, 8, 40, 8, 916, DateTimeKind.Unspecified).AddTicks(2256), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "user_subscriptions",
                columns: new[] { "id", "bought_at", "expires_at", "status", "subscription_id", "transaction_id", "user_id" },
                values: new object[] { -2, new DateTimeOffset(new DateTime(2025, 4, 3, 8, 40, 8, 916, DateTimeKind.Unspecified).AddTicks(1903), new TimeSpan(0, 3, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 5, 5, 8, 40, 8, 916, DateTimeKind.Unspecified).AddTicks(1929), new TimeSpan(0, 3, 0, 0, 0)), 0, 1, null, -1L });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -2L,
                column: "birth_day",
                value: new DateOnly(2000, 4, 5));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -1L,
                column: "birth_day",
                value: new DateOnly(2005, 4, 5));

            migrationBuilder.CreateIndex(
                name: "ix_user_subscriptions_user_id",
                table: "user_subscriptions",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_subscriptions",
                table: "user_subscriptions");

            migrationBuilder.DropIndex(
                name: "ix_user_subscriptions_user_id",
                table: "user_subscriptions");

            migrationBuilder.DeleteData(
                table: "user_subscriptions",
                keyColumn: "id",
                keyColumnType: "integer",
                keyValue: -2);

            migrationBuilder.DropColumn(
                name: "id",
                table: "user_subscriptions");

            migrationBuilder.DropColumn(
                name: "status",
                table: "user_subscriptions");

            migrationBuilder.DropColumn(
                name: "transaction_id",
                table: "user_subscriptions");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_subscriptions",
                table: "user_subscriptions",
                columns: new[] { "user_id", "subscription_id" });

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 7, 4, 14, 6, 18, 436, DateTimeKind.Unspecified).AddTicks(8666), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "favourite_contents",
                keyColumns: new[] { "content_id", "user_id" },
                keyValues: new object[] { -1L, -1L },
                column: "added_at",
                value: new DateTimeOffset(new DateTime(2024, 7, 4, 13, 26, 18, 436, DateTimeKind.Unspecified).AddTicks(8650), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "reviews",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 7, 4, 13, 56, 18, 436, DateTimeKind.Unspecified).AddTicks(8658), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "user_subscriptions",
                columns: new[] { "subscription_id", "user_id", "bought_at", "expires_at" },
                values: new object[] { 1, -1L, new DateTimeOffset(new DateTime(2024, 7, 2, 13, 56, 18, 436, DateTimeKind.Unspecified).AddTicks(8423), new TimeSpan(0, 3, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 8, 3, 13, 56, 18, 436, DateTimeKind.Unspecified).AddTicks(8463), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -2L,
                column: "birth_day",
                value: new DateOnly(1999, 7, 4));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: -1L,
                column: "birth_day",
                value: new DateOnly(2004, 7, 4));
        }
    }
}
