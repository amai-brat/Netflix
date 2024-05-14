using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "content_types",
                columns: new[] { "id", "content_type_name" },
                values: new object[,]
                {
                    { -3, "Мультфильм" },
                    { -2, "Сериал" },
                    { -1, "Фильм" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { -3, "криминал" },
                    { -2, "драма" },
                    { -1, "триллер" }
                });

            migrationBuilder.InsertData(
                table: "professions",
                columns: new[] { "id", "profession_name" },
                values: new object[] { -1, "Актер" });

            migrationBuilder.InsertData(
                table: "subscriptions",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Все фильмы на сервисе Netflix будут доступны после приобретения этой подписки", "Фильмы" },
                    { 2, "Все сериалы только в этой подписке", "Сериалы" },
                    { 3, "Мультфильмы для всех возрастов только в данной подписке", "Мультфильмы" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "birth_day", "email", "nickname", "password", "profile_picture_url", "role" },
                values: new object[,]
                {
                    { -2L, new DateOnly(1999, 3, 25), "testEmail2@gmail.com", "testUser2", "testPassword1337;", "https://st.kp.yandex.net/images/actor_iphone/iphone360_25584.jpg", "user" },
                    { -1L, new DateOnly(2004, 3, 25), "testEmail@gmail.com", "testUser", "testPassword228;", "https://i.pinimg.com/originals/2b/64/2f/2b642f9183fa80b8c47a9d8f8971eb4d.jpg", "user" }
                });

            migrationBuilder.InsertData(
                table: "content_bases",
                columns: new[] { "id", "age_ratings_age", "age_ratings_age_mpaa", "budget_budget_currency_name", "budget_budget_value", "ratings_imdb_rating", "ratings_kinopoisk_rating", "ratings_local_rating", "trailer_info_name", "trailer_info_url", "content_type_id", "country", "description", "name", "poster_url", "slogan" },
                values: new object[] { -1L, 18, "R", "$", 63000000, 8.8f, 8.668f, 0f, "Theatrical Trailer(HD Fan Remaster)", "https://www.youtube.com/embed/6JnN1DmbqoU", -1, "США", "Сотрудник страховой компании страдает хронической бессонницей и отчаянно пытается вырваться из мучительно скучной жизни. Однажды в очередной командировке он встречает некоего Тайлера Дёрдена — харизматического торговца мылом с извращенной философией. Тайлер уверен, что самосовершенствование — удел слабых, а единственное, ради чего стоит жить, — саморазрушение.\n\nПроходит немного времени, и вот уже новые друзья лупят друг друга почем зря на стоянке перед баром, и очищающий мордобой доставляет им высшее блаженство. Приобщая других мужчин к простым радостям физической жестокости, они основывают тайный Бойцовский клуб, который начинает пользоваться невероятной популярностью.", "Бойцовский клуб", "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig", "Интриги. Хаос. Мыло" });

            migrationBuilder.InsertData(
                table: "user_subscriptions",
                columns: new[] { "subscription_id", "user_id", "bought_at", "expires_at" },
                values: new object[] { 1, -1L, new DateTimeOffset(new DateTime(2024, 3, 23, 12, 41, 22, 134, DateTimeKind.Unspecified).AddTicks(9768), new TimeSpan(0, 3, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 4, 24, 12, 41, 22, 134, DateTimeKind.Unspecified).AddTicks(9806), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "content_base_genre",
                columns: new[] { "content_base_id", "genre_id" },
                values: new object[] { -1L, -1 });

            migrationBuilder.InsertData(
                table: "content_base_subscription",
                columns: new[] { "accessible_content_id", "subscription_id" },
                values: new object[] { -1L, 1 });

            migrationBuilder.InsertData(
                table: "favourite_contents",
                columns: new[] { "content_id", "user_id", "added_at" },
                values: new object[] { -1L, -1L, new DateTimeOffset(new DateTime(2024, 3, 25, 12, 11, 22, 134, DateTimeKind.Unspecified).AddTicks(9973), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "movie_contents",
                columns: new[] { "id", "movie_length", "release_date", "video_url" },
                values: new object[] { -1L, 139L, new DateOnly(1999, 9, 10), "/movie/{id}/res/{res}/output" });

            migrationBuilder.InsertData(
                table: "person_in_contents",
                columns: new[] { "id", "content_id", "name", "profession_id" },
                values: new object[,]
                {
                    { -2, -1L, "Эдвард Нортон", -1 },
                    { -1, -1L, "Брэд Питт", -1 }
                });

            migrationBuilder.InsertData(
                table: "reviews",
                columns: new[] { "id", "content_id", "is_positive", "score", "text", "user_id", "written_at" },
                values: new object[] { -1L, -1L, true, 9, "Невероятный фильм всем рекомендую, очень хороший фильм. Обожаю этот фильм не знаю, что еще сказать. Нет знаешь, нет я не знаю. Ты понял? Скажи! Мы один человек?", -1L, new DateTimeOffset(new DateTime(2024, 3, 25, 12, 41, 22, 134, DateTimeKind.Unspecified).AddTicks(9984), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "comments",
                columns: new[] { "id", "review_id", "text", "user_id", "written_at" },
                values: new object[] { -1L, -1L, "Полностью согласен", -2L, new DateTimeOffset(new DateTime(2024, 3, 25, 12, 51, 22, 134, DateTimeKind.Unspecified).AddTicks(9994), new TimeSpan(0, 3, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "users_reviews",
                columns: new[] { "review_id", "user_id", "is_liked" },
                values: new object[] { -1L, -1L, true });

            migrationBuilder.InsertData(
                table: "comment_user",
                columns: new[] { "scored_by_users_id", "scored_comments_id" },
                values: new object[] { -1L, -1L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "comment_user",
                keyColumns: new[] { "scored_by_users_id", "scored_comments_id" },
                keyValues: new object[] { -1L, -1L });

            migrationBuilder.DeleteData(
                table: "content_base_genre",
                keyColumns: new[] { "content_base_id", "genre_id" },
                keyValues: new object[] { -1L, -1 });

            migrationBuilder.DeleteData(
                table: "content_base_subscription",
                keyColumns: new[] { "accessible_content_id", "subscription_id" },
                keyValues: new object[] { -1L, 1 });

            migrationBuilder.DeleteData(
                table: "content_types",
                keyColumn: "id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "content_types",
                keyColumn: "id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "favourite_contents",
                keyColumns: new[] { "content_id", "user_id" },
                keyValues: new object[] { -1L, -1L });

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "movie_contents",
                keyColumn: "id",
                keyValue: -1L);

            migrationBuilder.DeleteData(
                table: "person_in_contents",
                keyColumn: "id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "person_in_contents",
                keyColumn: "id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "user_subscriptions",
                keyColumns: new[] { "subscription_id", "user_id" },
                keyValues: new object[] { 1, -1L });

            migrationBuilder.DeleteData(
                table: "users_reviews",
                keyColumns: new[] { "review_id", "user_id" },
                keyValues: new object[] { -1L, -1L });

            migrationBuilder.DeleteData(
                table: "comments",
                keyColumn: "id",
                keyValue: -1L);

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "professions",
                keyColumn: "id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "subscriptions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "reviews",
                keyColumn: "id",
                keyValue: -1L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: -2L);

            migrationBuilder.DeleteData(
                table: "content_bases",
                keyColumn: "id",
                keyValue: -1L);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: -1L);

            migrationBuilder.DeleteData(
                table: "content_types",
                keyColumn: "id",
                keyValue: -1);
        }
    }
}
