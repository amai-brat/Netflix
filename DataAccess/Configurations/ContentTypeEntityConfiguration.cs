using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class ContentTypeEntityConfiguration : IEntityTypeConfiguration<ContentType>
	{
		public void Configure(EntityTypeBuilder<ContentType> builder)
		{
			builder.HasKey(ct => ct.Id);

			builder.HasIndex(ct => ct.ContentTypeName)
				.IsUnique();
		}
	}
}
