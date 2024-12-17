using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupportPersistentAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class FileTypesFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "file_types",
                keyColumn: "id",
                keyValue: 1,
                column: "type",
                value: "image");

            migrationBuilder.UpdateData(
                table: "file_types",
                keyColumn: "id",
                keyValue: 2,
                column: "type",
                value: "audio");

            migrationBuilder.UpdateData(
                table: "file_types",
                keyColumn: "id",
                keyValue: 3,
                column: "type",
                value: "video");

            migrationBuilder.UpdateData(
                table: "file_types",
                keyColumn: "id",
                keyValue: 4,
                column: "type",
                value: "file");

            migrationBuilder.UpdateData(
                table: "file_types",
                keyColumn: "id",
                keyValue: 5,
                column: "type",
                value: "document");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "file_types",
                keyColumn: "id",
                keyValue: 1,
                column: "type",
                value: "Картинка");

            migrationBuilder.UpdateData(
                table: "file_types",
                keyColumn: "id",
                keyValue: 2,
                column: "type",
                value: "Аудио");

            migrationBuilder.UpdateData(
                table: "file_types",
                keyColumn: "id",
                keyValue: 3,
                column: "type",
                value: "Видео");

            migrationBuilder.UpdateData(
                table: "file_types",
                keyColumn: "id",
                keyValue: 4,
                column: "type",
                value: "Файл");

            migrationBuilder.UpdateData(
                table: "file_types",
                keyColumn: "id",
                keyValue: 5,
                column: "type",
                value: "Документ");
        }
    }
}
