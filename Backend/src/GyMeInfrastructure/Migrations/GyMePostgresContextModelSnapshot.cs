﻿// <auto-generated />
using System;
using GyMeInfrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GyMeInfrastructure.Migrations
{
    [DbContext(typeof(GyMePostgresContext))]
    partial class GyMePostgresContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.2.23128.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GyMeCore.Models.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<Guid>("SimpleExerciseId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SimpleExerciseId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.CommentReaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Emoji")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ReactionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("UserId");

                    b.ToTable("CommentReactions");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.Exercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ExercisesType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.ExtendedUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ImportancePremium")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Premium")
                        .HasColumnType("boolean");

                    b.Property<bool>("PrivateAccount")
                        .HasColumnType("boolean");

                    b.Property<string>("ProfilePictureUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("ExtendedUsers");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.Reaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Emoji")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("ReactionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SimpleExerciseId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SimpleExerciseId");

                    b.HasIndex("UserId");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.ResourcesAddresses", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ReactionImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ResourcesAddresses");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.Series", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("NumberOfRepetitions")
                        .HasMaxLength(1000)
                        .HasColumnType("integer");

                    b.Property<Guid>("SimpleExerciseId")
                        .HasColumnType("uuid");

                    b.Property<int>("Weight")
                        .HasMaxLength(100000)
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SimpleExerciseId");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.SimpleExercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("uuid");

                    b.Property<string>("ExerciseType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("UserId");

                    b.ToTable("SimpleExercises");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("AccountProvider")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime>("LastRefreshMainPage")
                        .HasColumnType("timestamp with time zone");

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
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<bool>("Valid")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.UserFriend", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FriendId")
                        .HasColumnType("uuid");

                    b.Property<int>("FriendStatus")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "FriendId");

                    b.HasIndex("FriendId");

                    b.ToTable("UserFriends");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.Comment", b =>
                {
                    b.HasOne("GyMeCore.Models.Entities.SimpleExercise", "SimpleExercise")
                        .WithMany("Comments")
                        .HasForeignKey("SimpleExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GyMeCore.Models.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SimpleExercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.CommentReaction", b =>
                {
                    b.HasOne("GyMeCore.Models.Entities.Comment", "Comment")
                        .WithMany("CommentReactions")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GyMeCore.Models.Entities.User", "User")
                        .WithMany("CommentReactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.Exercise", b =>
                {
                    b.HasOne("GyMeCore.Models.Entities.User", "User")
                        .WithMany("Exercises")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.ExtendedUser", b =>
                {
                    b.HasOne("GyMeCore.Models.Entities.User", "User")
                        .WithOne("ExtendedUser")
                        .HasForeignKey("GyMeCore.Models.Entities.ExtendedUser", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.Reaction", b =>
                {
                    b.HasOne("GyMeCore.Models.Entities.SimpleExercise", "SimpleExercise")
                        .WithMany("Reactions")
                        .HasForeignKey("SimpleExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GyMeCore.Models.Entities.User", "User")
                        .WithMany("Reactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SimpleExercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.ResourcesAddresses", b =>
                {
                    b.HasOne("GyMeCore.Models.Entities.User", "User")
                        .WithOne("SetResourcesAddresses")
                        .HasForeignKey("GyMeCore.Models.Entities.ResourcesAddresses", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.Series", b =>
                {
                    b.HasOne("GyMeCore.Models.Entities.SimpleExercise", "SimpleExercise")
                        .WithMany("Series")
                        .HasForeignKey("SimpleExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SimpleExercise");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.SimpleExercise", b =>
                {
                    b.HasOne("GyMeCore.Models.Entities.Exercise", "Exercise")
                        .WithMany("ConcreteExercise")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GyMeCore.Models.Entities.User", "User")
                        .WithMany("SimpleExercises")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.UserFriend", b =>
                {
                    b.HasOne("GyMeCore.Models.Entities.User", "Friend")
                        .WithMany("InverseFriends")
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GyMeCore.Models.Entities.User", "User")
                        .WithMany("Friends")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.Comment", b =>
                {
                    b.Navigation("CommentReactions");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.Exercise", b =>
                {
                    b.Navigation("ConcreteExercise");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.SimpleExercise", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Reactions");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("GyMeCore.Models.Entities.User", b =>
                {
                    b.Navigation("CommentReactions");

                    b.Navigation("Comments");

                    b.Navigation("Exercises");

                    b.Navigation("ExtendedUser");

                    b.Navigation("Friends");

                    b.Navigation("InverseFriends");

                    b.Navigation("Reactions");

                    b.Navigation("SetResourcesAddresses")
                        .IsRequired();

                    b.Navigation("SimpleExercises");
                });
#pragma warning restore 612, 618
        }
    }
}
