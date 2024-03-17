using Microsoft.EntityFrameworkCore;
using Domain;
using DataAccess.Configurations;

namespace DataAccess
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<User> Users => Set<User>();

		public DbSet<FavouriteContent> FavouriteContents => Set<FavouriteContent>();

		public DbSet<ContentBase> ContentBases => Set<ContentBase>();
		public DbSet<MovieContent> MovieContents => Set<MovieContent>();
		public DbSet<SerialContent> SerialContents => Set<SerialContent>();

		public DbSet<ContentType> ContentTypes => Set<ContentType>();
		public DbSet<SeasonInfo> SeasonInfos => Set<SeasonInfo>();
		public DbSet<Genre> Genres => Set<Genre>();

		public DbSet<PersonInContent> PersonInContents => Set<PersonInContent>();
		public DbSet<Profession> Professions => Set<Profession>();

		public DbSet<UsersReviews> UsersReviews => Set<UsersReviews>();
		public DbSet<Review> Reviews => Set<Review>();
		public DbSet<Comment> Comments => Set<Comment>();

		public DbSet<Subscription> Subscriptions => Set<Subscription>();



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new CommentEntityConfiguration());
			modelBuilder.ApplyConfiguration(new ContentBaseEntityConfiguration());
			modelBuilder.ApplyConfiguration(new ContentTypeEntityConfiguration());
			modelBuilder.ApplyConfiguration(new FavouriteContentEntityConfiguration());
			modelBuilder.ApplyConfiguration(new MovieContentEntityConfiguration());
			modelBuilder.ApplyConfiguration(new PersonInContentEntityConfiguration());
			modelBuilder.ApplyConfiguration(new ReviewEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SerialContentEntityConfiguration());
			modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
			modelBuilder.ApplyConfiguration(new UsersReviewsEntityConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}
