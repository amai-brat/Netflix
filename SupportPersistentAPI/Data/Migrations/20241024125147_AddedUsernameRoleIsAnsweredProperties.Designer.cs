﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SupportPersistentAPI.Data;

#nullable disable

namespace SupportPersistentAPI.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241024125147_AddedUsernameRoleIsAnsweredProperties")]
    partial class AddedUsernameRoleIsAnsweredProperties
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SupportAPI.Data.Entities.SupportChatMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("ChatSessionId")
                        .HasColumnType("bigint")
                        .HasColumnName("chat_session_id");

                    b.Property<DateTimeOffset>("DateTimeSent")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_time_sent");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.Property<long>("SenderId")
                        .HasColumnType("bigint")
                        .HasColumnName("sender_id");

                    b.Property<string>("SenderName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sender_name");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.HasKey("Id")
                        .HasName("pk_support_chat_messages");

                    b.HasIndex("ChatSessionId")
                        .HasDatabaseName("ix_support_chat_messages_chat_session_id");

                    b.ToTable("support_chat_messages", (string)null);
                });

            modelBuilder.Entity("SupportAPI.Data.Entities.SupportChatSession", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsAnswered")
                        .HasColumnType("boolean")
                        .HasColumnName("is_answered");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_support_chat_sessions");

                    b.ToTable("support_chat_sessions", (string)null);
                });

            modelBuilder.Entity("SupportAPI.Data.Entities.SupportChatMessage", b =>
                {
                    b.HasOne("SupportAPI.Data.Entities.SupportChatSession", "ChatSession")
                        .WithMany("ChatMessages")
                        .HasForeignKey("ChatSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_support_chat_messages_support_chat_sessions_chat_session_id");

                    b.Navigation("ChatSession");
                });

            modelBuilder.Entity("SupportAPI.Data.Entities.SupportChatSession", b =>
                {
                    b.Navigation("ChatMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
