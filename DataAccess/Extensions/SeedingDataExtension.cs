using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Extensions
{
	public static class SeedingDataExtension
	{
		public static void SeedWithTestData(this ModelBuilder modelBuilder)
		{
			var subscriptions = new List<Subscription>()
			{
				new Subscription()
				{
					Id = 1, Name = "Фильмы", MaxResolution = 2160, Price = 300,
					Description = "Все фильмы на сервисе Netflix будут доступны после приобретения этой подписки"
				},
				new Subscription()
				{
					Id = 2, Name = "Сериалы", MaxResolution = 1080, Price = 350,
					Description = "Все сериалы только в этой подписке"
				},
				new Subscription()
				{
					Id = 3, Name = "Мультфильмы", MaxResolution = 720, Price = 228,
					Description = "Мультфильмы для всех возрастов только в данной подписке"
				}
			};

			var contentTypes = new List<ContentType>()
			{
				new ContentType() { Id = -1, ContentTypeName = "Фильм" },
				new ContentType() { Id = -2, ContentTypeName = "Сериал" },
				new ContentType() { Id = -3, ContentTypeName = "Мультфильм" }
			};

			var genres = new List<Genre>()
			{
				new Genre() { Id = -1, Name = "триллер" },
				new Genre() { Id = -2, Name = "драма" },
				new Genre() { Id = -3, Name = "криминал"}
			};

			var testUser = new User()
			{
				Id = -1,
				Nickname = "testUser",
				Email = "testEmail@gmail.com",
				BirthDay = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
				ProfilePictureUrl = "https://i.pinimg.com/originals/2b/64/2f/2b642f9183fa80b8c47a9d8f8971eb4d.jpg"
			};

			var testUser2 = new User()
			{
				Id = -2,
				Nickname = "testUser2",
				Email = "testEmail2@gmail.com",
				BirthDay = DateOnly.FromDateTime(DateTime.Now.AddYears(-25)),
				ProfilePictureUrl = "https://st.kp.yandex.net/images/actor_iphone/iphone360_25584.jpg",

			};

			var userToSubscription = new UserSubscription()
			{
				SubscriptionId = 1,
				UserId = -1,
				BoughtAt = DateTimeOffset.Now.AddDays(-2),
				ExpiresAt = DateTimeOffset.Now.AddDays(30)
			};

			var testMovieContent = new MovieContent()
			{
				Id = -1,
				Name = "Бойцовский клуб",
				Description = "Сотрудник страховой компании страдает хронической бессонницей и отчаянно пытается вырваться из мучительно скучной жизни. Однажды в очередной командировке он встречает некоего Тайлера Дёрдена — харизматического торговца мылом с извращенной философией. Тайлер уверен, что самосовершенствование — удел слабых, а единственное, ради чего стоит жить, — саморазрушение.\n\nПроходит немного времени, и вот уже новые друзья лупят друг друга почем зря на стоянке перед баром, и очищающий мордобой доставляет им высшее блаженство. Приобщая других мужчин к простым радостям физической жестокости, они основывают тайный Бойцовский клуб, который начинает пользоваться невероятной популярностью.",
				MovieLength = 139,
				ContentTypeId = -1,
				Slogan = "Интриги. Хаос. Мыло",
				Country = "США",
				ReleaseDate = DateOnly.Parse("1999-09-10"),
				PosterUrl = "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig",
				VideoUrl = "/movie/{id}/res/{res}/output"
			};

			var favouriteContent = new FavouriteContent()
			{
				UserId = -1,
				ContentId = -1,
				AddedAt = DateTimeOffset.Now.AddMinutes(-30)
			};

			var review = new Review()
			{
				Id = -1,
				UserId = -1,
				ContentId = -1,
				IsPositive = true,
				Score = 9,
				WrittenAt = DateTimeOffset.Now,
				Text = "Невероятный фильм всем рекомендую, очень хороший фильм. Обожаю этот фильм не знаю, что еще сказать. Нет знаешь, нет я не знаю. Ты понял? Скажи! Мы один человек?",
			};

			var userToReview = new UsersReviews()
			{
				UserId = -1,
				ReviewId = -1,
				IsLiked = true
			};

			var comment = new Comment()
			{
				Id = -1,
				ReviewId = -1,
				UserId = -2,
				Text = "Полностью согласен",
				WrittenAt = DateTimeOffset.Now.AddMinutes(10)
			};

			var profession = new Profession()
			{
				Id = -1,
				ProfessionName = "Актер"
			};

			var personInContent1 = new PersonInContent()
			{
				Id = -1,
				ContentId = -1,
				ProfessionId = -1,
				Name = "Брэд Питт",
			};

			var personInContent2 = new PersonInContent()
			{
				Id = -2,
				ContentId = -1,
				ProfessionId = -1,
				Name = "Эдвард Нортон",
			};

			modelBuilder.Entity<Genre>().HasData(genres);
			modelBuilder.Entity<ContentType>().HasData(contentTypes);
			modelBuilder.Entity<Subscription>().HasData(subscriptions);
			modelBuilder.Entity<User>().HasData(testUser, testUser2);
			modelBuilder.Entity<UserSubscription>().HasData(userToSubscription);

			modelBuilder.Entity<MovieContent>().HasData(testMovieContent);
			modelBuilder.Entity<ContentBase>()
				.HasMany(cb => cb.AllowedSubscriptions)
				.WithMany(a => a.AccessibleContent)
				.UsingEntity<Dictionary<string, object>>(
					je =>
					{
						je.HasData(
							new { SubscriptionId = 1, AccessibleContentId = -1L }
						);
					});
			modelBuilder.Entity<ContentBase>()
				.HasMany(cb => cb.Genres)
				.WithMany(a => a.Contents)
				.UsingEntity<Dictionary<string, object>>(
					je =>
					{
						je.HasData(
							new { GenreId = -1, ContentBaseId = -1L }
						);
					});

			modelBuilder.Entity<ContentBase>().OwnsOne(cb => cb.Budget)
				.HasData(new 
				{
					ContentBaseId = -1L,
					BudgetCurrencyName = "$",
					BudgetValue = 63000000
				});
			modelBuilder.Entity<ContentBase>().OwnsOne(cb => cb.Ratings)
				.HasData(new
				{
					ContentBaseId = -1L,
					KinopoiskRating = 8.668F,
					ImdbRating = 8.8F,
					LocalRating = 0F
				});
			modelBuilder.Entity<ContentBase>().OwnsOne(cb => cb.AgeRatings)
				.HasData(new
				{
					ContentBaseId = -1L,
					Age = 18,
					AgeMpaa = "R"
				});
			modelBuilder.Entity<ContentBase>().OwnsOne(cb => cb.TrailerInfo)
				.HasData(new
				{
					ContentBaseId = -1L,
					Name = "Theatrical Trailer(HD Fan Remaster)",
					Url = "https://www.youtube.com/embed/6JnN1DmbqoU"
				});

			modelBuilder.Entity<FavouriteContent>().HasData(favouriteContent);
			modelBuilder.Entity<Review>().HasData(review);
			modelBuilder.Entity<UsersReviews>().HasData(userToReview);

			modelBuilder.Entity<Comment>().HasData(comment);
			modelBuilder.Entity<Comment>().HasMany(c => c.ScoredByUsers)
				.WithMany(u => u.ScoredComments)
				.UsingEntity<Dictionary<string, object>>(
					je =>
					{
						je.HasData(
							new { ScoredByUsersId = -1L, ScoredCommentsId = -1L }
						);
					});

			modelBuilder.Entity<Profession>().HasData(profession);
			modelBuilder.Entity<PersonInContent>().HasData(personInContent1, personInContent2);

			var currencies = new List<Currency>()
			{
				new()
				{
					Id = 1,
					Name = "RUB"
				},
				new()
				{
					Id = 2,
					Name = "USD" 
				}
			};

			modelBuilder.Entity<Currency>().HasData(currencies);
		}
	}
}
