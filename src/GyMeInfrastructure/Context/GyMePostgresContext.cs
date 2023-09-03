﻿using GymAppCore.Models.Entities;
using GymAppCore.Models.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Options;

public class GyMePostgresContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ExtendedUser> ExtendedUsers { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<SimpleExercise> SimpleExercises { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<UserFriend> UserFriends { get; set; }

    public GyMePostgresContext(DbContextOptions<GyMePostgresContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UserFriendConfig).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(EntitiesConfig).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(SimpleExerciseConfig).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(UserConfig).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(ExtendedUser).Assembly);
    }
}