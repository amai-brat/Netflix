﻿using Microsoft.EntityFrameworkCore;
using SupportPersistentAPI.Data.Entities;
using FileInfo = SupportPersistentAPI.Data.Entities.FileInfo;

namespace SupportPersistentAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<SupportChatMessage> SupportChatMessages => Set<SupportChatMessage>();
        public DbSet<SupportChatSession> SupportChatSessions => Set<SupportChatSession>();
        public DbSet<FileInfo> FileInfos => Set<FileInfo>();
        public DbSet<FileTypeEntity> FileTypes => Set<FileTypeEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupportChatSession>()
                .HasMany(scs => scs.ChatMessages)
                .WithOne(scm => scm.ChatSession)
                .HasForeignKey(scm => scm.ChatSessionId);

            modelBuilder.Entity<FileInfo>()
                .Property(fi => fi.Type)
                .HasConversion<int>();

            modelBuilder.Entity<FileInfo>()
                .HasOne(fi => fi.TypeLookup)
                .WithMany()
                .HasForeignKey(fi => fi.TypeId);

            modelBuilder.Entity<FileTypeEntity>().HasData(
                new FileTypeEntity(){Id = 1, Type = "image"},
                new FileTypeEntity(){Id = 2, Type = "audio"},
                new FileTypeEntity(){Id = 3, Type = "video"},
                new FileTypeEntity(){Id = 4, Type = "file"},
                new FileTypeEntity(){Id = 5, Type = "document"}
            );
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
