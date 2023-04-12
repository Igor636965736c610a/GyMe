﻿// <auto-generated />
using System;
using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GymAppInfrastructure.Migrations
{
    [DbContext(typeof(GymAppContext))]
    [Migration("20230412021802_Initial3")]
    partial class Initial3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.2.23128.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GymAppCore.Models.Entities.Exercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("ExercisesType")
                        .HasColumnType("integer");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.Premium", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Importance")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("PremiumAccount")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Premiums");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.Series", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("NumberOfRepetitions")
                        .HasColumnType("integer");

                    b.Property<Guid>("SimpleExerciseId")
                        .HasColumnType("uuid");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SimpleExerciseId");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.SimpleExercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("uuid");

                    b.Property<string>("SeriesString")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("UserId");

                    b.ToTable("SimpleExercises");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.UserFriend", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FriendId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "FriendId");

                    b.HasIndex("FriendId");

                    b.ToTable("UserFriends");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.Exercise", b =>
                {
                    b.HasOne("GymAppCore.Models.Entities.User", "User")
                        .WithMany("Exercises")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.Premium", b =>
                {
                    b.HasOne("GymAppCore.Models.Entities.User", "User")
                        .WithOne("Premium")
                        .HasForeignKey("GymAppCore.Models.Entities.Premium", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.Series", b =>
                {
                    b.HasOne("GymAppCore.Models.Entities.SimpleExercise", "SimpleExercise")
                        .WithMany("Series")
                        .HasForeignKey("SimpleExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SimpleExercise");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.SimpleExercise", b =>
                {
                    b.HasOne("GymAppCore.Models.Entities.Exercise", "Exercise")
                        .WithMany("ConcreteExercise")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymAppCore.Models.Entities.User", "User")
                        .WithMany("SimpleExercises")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.UserFriend", b =>
                {
                    b.HasOne("GymAppCore.Models.Entities.User", "Friend")
                        .WithMany("InverseFriends")
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymAppCore.Models.Entities.User", "User")
                        .WithMany("Friends")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.Exercise", b =>
                {
                    b.Navigation("ConcreteExercise");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.SimpleExercise", b =>
                {
                    b.Navigation("Series");
                });

            modelBuilder.Entity("GymAppCore.Models.Entities.User", b =>
                {
                    b.Navigation("Exercises");

                    b.Navigation("Friends");

                    b.Navigation("InverseFriends");

                    b.Navigation("Premium")
                        .IsRequired();

                    b.Navigation("SimpleExercises");
                });
#pragma warning restore 612, 618
        }
    }
}
