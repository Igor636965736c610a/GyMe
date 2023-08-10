using GymAppCore.Models.Entities;
using GymAppCore.Models.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Context;

public class GymAppContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<SimpleExercise> SimpleExercises { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<UserFriend> UserFriends { get; set; }

    public GymAppContext(DbContextOptions<GymAppContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UserFriendConfig).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(EntitiesConfig).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(SimpleExerciseConfig).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
        // builder.Entity<User>(x => 
        //     x.HasMany(e => e.Exercises)
        //         .WithOne(e => e.User)
        //         .HasForeignKey(e => e.UserId)
        //         .HasPrincipalKey(e => e.Id));
        
        // builder.Entity<User>(x => 
        //     x.HasMany(e => e.SimpleExercises)
        //         .WithOne(e => e.User)
        //         .HasForeignKey(e => e.UserId)
        //         .HasPrincipalKey(e => e.Id));
        
        // builder.Entity<Exercise>(x => 
        //     x.HasMany(e => e.ConcreteExercise)
        //         .WithOne(e => e.Exercise)
        //         .HasForeignKey(e => e.ExerciseId)
        //         .HasPrincipalKey(e => e.Id));
        
        // builder.Entity<UserFriend>()
        //     .HasKey(x => new { x.UserId, x.FriendId });
        //
        // builder.Entity<UserFriend>()
        //     .HasOne(e => e.User)
        //     .WithMany(e => e.Friends)
        //     .HasForeignKey(e => e.UserId);
        //
        // builder.Entity<UserFriend>()
        //     .HasOne(e => e.Friend)
        //     .WithMany(e => e.InverseFriends)
        //     .HasForeignKey(e => e.FriendId);
        
        
        // builder.Entity<SimpleExercise>(x => 
        //     x.HasMany(e => e.Series)
        //         .WithOne(e => e.SimpleExercise)
        //         .HasForeignKey(e => e.SimpleExerciseId)
        //         .HasPrincipalKey(e => e.Id));
    }
}