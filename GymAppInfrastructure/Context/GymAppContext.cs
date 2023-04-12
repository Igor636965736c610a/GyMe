using GymAppCore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Context;

public class GymAppContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<SimpleExercise> SimpleExercises { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<Premium> Premiums { get; set; }
    public DbSet<UserFriend> UserFriends { get; set; }

    public GymAppContext(DbContextOptions<GymAppContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(x => 
            x.HasMany(e => e.Exercises).WithOne(e => e.User).HasForeignKey(e => e.UserId).HasPrincipalKey(e => e.Id));
        builder.Entity<User>(x => 
            x.HasMany(e => e.SimpleExercises).WithOne(e => e.User).HasForeignKey(e => e.UserId).HasPrincipalKey(e => e.Id));
        builder.Entity<User>(x => 
            x.HasOne(e => e.Premium).WithOne(e => e.User).HasForeignKey<Premium>(e => e.UserId));
        builder.Entity<UserFriend>()
            .HasKey(x => new { x.UserId, x.FriendId });
        builder.Entity<UserFriend>().HasOne(e => e.User).WithMany(e => e.Friends).HasForeignKey(e => e.UserId);
        builder.Entity<UserFriend>().HasOne(e => e.Friend).WithMany(e => e.InverseFriends)
            .HasForeignKey(e => e.FriendId);
        builder.Entity<Exercise>(x => 
            x.HasMany(e => e.ConcreteExercise).WithOne(e => e.Exercise));
        builder.Entity<SimpleExercise>(x => 
            x.HasMany(e => e.Series).WithOne(e => e.SimpleExercise));
    }
}