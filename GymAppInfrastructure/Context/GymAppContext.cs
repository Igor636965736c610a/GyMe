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
    
    public GymAppContext(DbContextOptions<GymAppContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(x => x.HasMany(e => e.Exercises).WithOne(e => e.User));
        builder.Entity<User>(x => x.HasMany(e => e.SimpleExercises).WithOne(e => e.User));
        builder.Entity<User>(x => x.HasOne(e => e.Premium).WithOne(e => e.User).HasForeignKey<Premium>(e => e.UserId));
        builder.Entity<Exercise>(x => x.HasMany(e => e.ConcreteExercise).WithOne(e => e.Exercise));
        builder.Entity<SimpleExercise>(x => x.HasMany(e => e.Series).WithOne(e => e.SimpleExercise));
    }
}