using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SupportPermanentS3Service.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "metadata",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_metadata", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "values",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_id = table.Column<long>(type: "bigint", nullable: false),
                    metadata_id = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_values", x => x.id);
                    table.ForeignKey(
                        name: "fk_values_files_file_id",
                        column: x => x.file_id,
                        principalTable: "files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_values_metadata_metadata_id",
                        column: x => x.metadata_id,
                        principalTable: "metadata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_values_file_id",
                table: "values",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "ix_values_metadata_id",
                table: "values",
                column: "metadata_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "values");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "metadata");
        }
    }
}
