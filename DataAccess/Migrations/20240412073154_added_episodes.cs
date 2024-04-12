using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class added_episodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_episode_season_infos_season_info_id",
                table: "episode");

            migrationBuilder.DropPrimaryKey(
                name: "pk_episode",
                table: "episode");

            migrationBuilder.RenameTable(
                name: "episode",
                newName: "episodes");

            migrationBuilder.RenameIndex(
                name: "ix_episode_season_info_id",
                table: "episodes",
                newName: "ix_episodes_season_info_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_episodes",
                table: "episodes",
                column: "id");

            migrationBuilder.UpdateData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 12, 10, 41, 53, 927, DateTimeKind.Unspecified).AddTicks(2730), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "favourite_contents",
                keyColumns: new[] { "content_id", "user_id" },
                keyValues: new object[] { -1L, -1L },
                column: "added_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 12, 10, 1, 53, 927, DateTimeKind.Unspecified).AddTicks(2713), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "reviews",
                keyColumn: "id",
                keyValue: -1L,
                column: "written_at",
                value: new DateTimeOffset(new DateTime(2024, 4, 12, 10, 31, 53, 927, DateTimeKind.Unspecified).AddTicks(2724), new TimeSpan(0, 3, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "user_subscriptions",
                keyColumns: new[] { "subscription_id", "user_id" },
                keyValues: new object[] { 1, -1L },
                columns: new[] { "bought_at", "expires_at" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 4, 10, 10, 31, 53, 927, DateTimeKind.Unspecified).AddTicks(2503), new TimeSpan(0, 3, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 5, 12, 10, 31, 53, 927, DateTimeKind.Unspecified).AddTicks(2541), new TimeSpan(0, 3, 0, 0, 0)) });

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

            migrationBuilder.AddForeignKey(
                name: "fk_episodes_season_infos_season_info_id",
                table: "episodes",
                column: "season_info_id",
                principalTable: "season_infos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_episodes_season_infos_season_info_id",
                table: "episodes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_episodes",
                table: "episodes");

            migrationBuilder.RenameTable(
                name: "episodes",
                newName: "episode");

            migrationBuilder.RenameIndex(
                name: "ix_episodes_season_info_id",
                table: "episode",
                newName: "ix_episode_season_info_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_episode",
                table: "episode",
                column: "id");

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

            migrationBuilder.AddForeignKey(
                name: "fk_episode_season_infos_season_info_id",
                table: "episode",
                column: "season_info_id",
                principalTable: "season_infos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
