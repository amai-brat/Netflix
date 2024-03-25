using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "content_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    content_type_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_content_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_genres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "professions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    profession_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_professions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscriptions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nickname = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false, defaultValue: "user"),
                    profile_picture_url = table.Column<string>(type: "text", nullable: true),
                    birth_day = table.Column<DateOnly>(type: "date", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "content_bases",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    slogan = table.Column<string>(type: "text", nullable: true),
                    poster_url = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: true),
                    content_type_id = table.Column<int>(type: "integer", nullable: false),
                    age_ratings_age = table.Column<int>(type: "integer", nullable: true),
                    age_ratings_age_mpaa = table.Column<string>(type: "text", nullable: true),
                    ratings_kinopoisk_rating = table.Column<float>(type: "real", nullable: true),
                    ratings_imdb_rating = table.Column<float>(type: "real", nullable: true),
                    ratings_local_rating = table.Column<float>(type: "real", nullable: true),
                    trailer_info_url = table.Column<string>(type: "text", nullable: true),
                    trailer_info_name = table.Column<string>(type: "text", nullable: true),
                    budget_budget_value = table.Column<int>(type: "integer", nullable: true),
                    budget_budget_currency_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_content_bases", x => x.id);
                    table.ForeignKey(
                        name: "fk_content_bases_content_types_content_type_id",
                        column: x => x.content_type_id,
                        principalTable: "content_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_subscriptions",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    subscription_id = table.Column<int>(type: "integer", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    bought_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_subscriptions", x => new { x.user_id, x.subscription_id });
                    table.ForeignKey(
                        name: "fk_user_subscriptions_subscriptions_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_subscriptions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "content_base_genre",
                columns: table => new
                {
                    genre_id = table.Column<int>(type: "integer", nullable: false),
                    content_base_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_content_base_genre", x => new { x.genre_id, x.content_base_id });
                    table.ForeignKey(
                        name: "fk_content_base_genre_content_bases_content_base_id",
                        column: x => x.content_base_id,
                        principalTable: "content_bases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_content_base_genre_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "content_base_subscription",
                columns: table => new
                {
                    subscription_id = table.Column<int>(type: "integer", nullable: false),
                    accessible_content_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_content_base_subscription", x => new { x.subscription_id, x.accessible_content_id });
                    table.ForeignKey(
                        name: "fk_content_base_subscription_content_bases_accessible_content_",
                        column: x => x.accessible_content_id,
                        principalTable: "content_bases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_content_base_subscription_subscriptions_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "favourite_contents",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    content_id = table.Column<long>(type: "bigint", nullable: false),
                    added_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_favourite_contents", x => new { x.user_id, x.content_id });
                    table.ForeignKey(
                        name: "fk_favourite_contents_content_bases_content_id",
                        column: x => x.content_id,
                        principalTable: "content_bases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_favourite_contents_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movie_contents",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    movie_length = table.Column<long>(type: "bigint", nullable: false),
                    video_url = table.Column<string>(type: "text", nullable: false),
                    release_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie_contents", x => x.id);
                    table.ForeignKey(
                        name: "fk_movie_contents_content_bases_id",
                        column: x => x.id,
                        principalTable: "content_bases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "person_in_contents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    content_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    profession_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_person_in_contents", x => x.id);
                    table.ForeignKey(
                        name: "fk_person_in_contents_content_bases_content_id",
                        column: x => x.content_id,
                        principalTable: "content_bases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_person_in_contents_professions_profession_id",
                        column: x => x.profession_id,
                        principalTable: "professions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    content_id = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    is_positive = table.Column<bool>(type: "boolean", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    written_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fk_reviews_content_bases_content_id",
                        column: x => x.content_id,
                        principalTable: "content_bases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reviews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "serial_contents",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    year_range_start = table.Column<DateOnly>(type: "date", nullable: false),
                    year_range_end = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serial_contents", x => x.id);
                    table.ForeignKey(
                        name: "fk_serial_contents_content_bases_id",
                        column: x => x.id,
                        principalTable: "content_bases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_serial_contents_serial_contents_id",
                        column: x => x.id,
                        principalTable: "content_bases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    review_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    written_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_comments_reviews_review_id",
                        column: x => x.review_id,
                        principalTable: "reviews",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_comments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users_reviews",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    review_id = table.Column<long>(type: "bigint", nullable: false),
                    is_liked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users_reviews", x => new { x.user_id, x.review_id });
                    table.ForeignKey(
                        name: "fk_users_reviews_reviews_review_id",
                        column: x => x.review_id,
                        principalTable: "reviews",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_reviews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "season_infos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    season_number = table.Column<int>(type: "integer", nullable: false),
                    serial_content_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_season_infos", x => x.id);
                    table.ForeignKey(
                        name: "fk_season_infos_serial_contents_serial_content_id",
                        column: x => x.serial_content_id,
                        principalTable: "serial_contents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment_user",
                columns: table => new
                {
                    scored_by_users_id = table.Column<long>(type: "bigint", nullable: false),
                    scored_comments_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comment_user", x => new { x.scored_by_users_id, x.scored_comments_id });
                    table.ForeignKey(
                        name: "fk_comment_user_comments_scored_comments_id",
                        column: x => x.scored_comments_id,
                        principalTable: "comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_comment_user_users_scored_by_users_id",
                        column: x => x.scored_by_users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "episode",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    episode_number = table.Column<int>(type: "integer", nullable: false),
                    video_url = table.Column<string>(type: "text", nullable: false),
                    season_info_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_episode", x => x.id);
                    table.ForeignKey(
                        name: "fk_episode_season_infos_season_info_id",
                        column: x => x.season_info_id,
                        principalTable: "season_infos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_comment_user_scored_comments_id",
                table: "comment_user",
                column: "scored_comments_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_review_id",
                table: "comments",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "ix_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_content_base_genre_content_base_id",
                table: "content_base_genre",
                column: "content_base_id");

            migrationBuilder.CreateIndex(
                name: "ix_content_base_subscription_accessible_content_id",
                table: "content_base_subscription",
                column: "accessible_content_id");

            migrationBuilder.CreateIndex(
                name: "ix_content_bases_content_type_id",
                table: "content_bases",
                column: "content_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_content_types_content_type_name",
                table: "content_types",
                column: "content_type_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_episode_season_info_id",
                table: "episode",
                column: "season_info_id");

            migrationBuilder.CreateIndex(
                name: "ix_favourite_contents_content_id",
                table: "favourite_contents",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "ix_person_in_contents_content_id",
                table: "person_in_contents",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "ix_person_in_contents_profession_id",
                table: "person_in_contents",
                column: "profession_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_content_id",
                table: "reviews",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "ix_reviews_user_id",
                table: "reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_season_infos_serial_content_id",
                table: "season_infos",
                column: "serial_content_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_subscriptions_subscription_id",
                table: "user_subscriptions",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_reviews_review_id",
                table: "users_reviews",
                column: "review_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comment_user");

            migrationBuilder.DropTable(
                name: "content_base_genre");

            migrationBuilder.DropTable(
                name: "content_base_subscription");

            migrationBuilder.DropTable(
                name: "episode");

            migrationBuilder.DropTable(
                name: "favourite_contents");

            migrationBuilder.DropTable(
                name: "movie_contents");

            migrationBuilder.DropTable(
                name: "person_in_contents");

            migrationBuilder.DropTable(
                name: "user_subscriptions");

            migrationBuilder.DropTable(
                name: "users_reviews");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "season_infos");

            migrationBuilder.DropTable(
                name: "professions");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "serial_contents");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "content_bases");

            migrationBuilder.DropTable(
                name: "content_types");
        }
    }
}
