using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SupportPersistentAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class FileInfoSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "file_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "file_infos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    type_id = table.Column<int>(type: "integer", nullable: false),
                    src = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    support_chat_message_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file_infos", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_infos_file_types_type_id",
                        column: x => x.type_id,
                        principalTable: "file_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_file_infos_support_chat_messages_support_chat_message_id",
                        column: x => x.support_chat_message_id,
                        principalTable: "support_chat_messages",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "file_types",
                columns: new[] { "id", "type" },
                values: new object[,]
                {
                    { 1, "Картинка" },
                    { 2, "Аудио" },
                    { 3, "Видео" },
                    { 4, "Файл" },
                    { 5, "Документ" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_file_infos_support_chat_message_id",
                table: "file_infos",
                column: "support_chat_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_file_infos_type_id",
                table: "file_infos",
                column: "type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "file_infos");

            migrationBuilder.DropTable(
                name: "file_types");
        }
    }
}
