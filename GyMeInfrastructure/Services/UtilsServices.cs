﻿using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Series;

namespace GymAppInfrastructure.Services;

internal static class UtilsServices
{
    internal static async Task<bool> CheckResourceAccessPermissions(Guid userIdFromJwt, Guid userIdFromResource, IUserRepo userRepo)
    {
        if (userIdFromJwt == userIdFromResource)
            return true;
        
        var user = await userRepo.Get(userIdFromResource);
        if (!user!.PrivateAccount)
            return true;

        var friendStatus = await userRepo.GetFriend(userIdFromJwt, userIdFromResource);
        
        return friendStatus is not null && friendStatus.FriendStatus == FriendStatus.Friend;
    }

    internal static List<Series> SeriesFromString(string? series, SimpleExercise simpleExercise)
    {
        if (series is null) return new List<Series>();
        var output = series
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(str => !string.IsNullOrWhiteSpace(str))
            .Select(ParseSeries)
            .Select(pair => new Series(simpleExercise, pair.second, pair.first))
            .ToList();

        if (output.Count > 30)
        {
            throw new InvalidOperationException("Maximum record count exceeded (30)");
        }

        return output;
    }

    private static (int first, int second) ParseSeries(string str)
    {
        string[] parts = str.Split('x');
        if (parts.Length != 2 || !int.TryParse(parts[0], out int first) ||
            !int.TryParse(parts[1], out int second))
        {
            throw new ArgumentException($"Invalid format: \"{str}\"");
        }

        if (parts[0].Length > 11 || parts[1].Length > 11)
        {
            throw new ArgumentException($"Maximum length exceeded: \"{str}\"");
        }

        return (first, second);
    }
}