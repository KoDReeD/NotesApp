﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotesApi;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NotesApi.Migrations
{
    [DbContext(typeof(ApplicatonDbContext))]
    partial class ApplicatonDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NotesApi.DbModels.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarPath")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastEntryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("NotesApi.DbModels.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("WhoCreatedId")
                        .HasColumnType("integer");

                    b.Property<int?>("WhoUpdatedId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WhoCreatedId");

                    b.HasIndex("WhoUpdatedId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("NotesApi.DbModels.NoteTags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("NoteId")
                        .HasColumnType("integer");

                    b.Property<int>("TagId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.HasIndex("TagId");

                    b.ToTable("NoteTags");
                });

            modelBuilder.Entity("NotesApi.DbModels.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ChangeStatusDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ChangeStatusIp")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBrowser")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedIp")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpiresDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("NotesApi.DbModels.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("NotesApi.DbModels.Note", b =>
                {
                    b.HasOne("NotesApi.DbModels.Account", "WhoCreated")
                        .WithMany("CreatedNotesList")
                        .HasForeignKey("WhoCreatedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotesApi.DbModels.Account", "WhoUpdated")
                        .WithMany("UpdatedNotesList")
                        .HasForeignKey("WhoUpdatedId");

                    b.Navigation("WhoCreated");

                    b.Navigation("WhoUpdated");
                });

            modelBuilder.Entity("NotesApi.DbModels.NoteTags", b =>
                {
                    b.HasOne("NotesApi.DbModels.Note", "Note")
                        .WithMany("Tags")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotesApi.DbModels.Tag", "Tag")
                        .WithMany("Notes")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Note");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("NotesApi.DbModels.RefreshToken", b =>
                {
                    b.HasOne("NotesApi.DbModels.Account", "Account")
                        .WithMany("Tokens")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("NotesApi.DbModels.Account", b =>
                {
                    b.Navigation("CreatedNotesList");

                    b.Navigation("Tokens");

                    b.Navigation("UpdatedNotesList");
                });

            modelBuilder.Entity("NotesApi.DbModels.Note", b =>
                {
                    b.Navigation("Tags");
                });

            modelBuilder.Entity("NotesApi.DbModels.Tag", b =>
                {
                    b.Navigation("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
