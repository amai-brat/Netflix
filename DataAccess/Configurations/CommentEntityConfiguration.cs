using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
	{
		public void Configure(EntityTypeBuilder<Comment> builder)
		{
			builder.HasKey(c => c.Id);

			builder.Property(c => c.Text).IsRequired();
			builder.Property(c => c.WrittenAt).IsRequired();
		}
	}
}
