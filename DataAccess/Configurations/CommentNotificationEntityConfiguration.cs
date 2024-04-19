using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class CommentNotificationEntityConfiguration : IEntityTypeConfiguration<CommentNotification>
{
    public void Configure(EntityTypeBuilder<CommentNotification> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.HasOne(c => c.Comment)
            .WithOne(c => c.CommentNotification)
            .HasForeignKey<Comment>(c => c.CommentNotificationId);
    }
}