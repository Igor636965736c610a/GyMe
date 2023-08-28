using GymAppInfrastructure.Options;

namespace GymAppInfrastructure.Repo;

internal static class UtilsRepo
{
    internal static async Task<bool> SaveDatabaseChanges(GyMePostgresContext gyMePostgresContext) {
        return await gyMePostgresContext.SaveChangesAsync() >= 0 ? true : false;
    }
}