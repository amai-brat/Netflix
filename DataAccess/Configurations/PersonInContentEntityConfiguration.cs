using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class PersonInContentEntityConfiguration : IEntityTypeConfiguration<PersonInContent>
	{
		public void Configure(EntityTypeBuilder<PersonInContent> builder)
		{
			builder.HasKey(p => p.Id);

			builder.HasOne(p => p.Content)
				.WithMany(c => c.PersonsInContent)
				.HasForeignKey(p => p.ContentId);

			builder.HasOne(p => p.Profession)
				.WithMany()
				.HasForeignKey(p => p.ProfessionId);
		}
	}
}
