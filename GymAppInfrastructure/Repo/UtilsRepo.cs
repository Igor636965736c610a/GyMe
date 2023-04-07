using GymAppInfrastructure.Context;

namespace GymAppInfrastructure.Repo;

internal static class UtilsRepo
{
    internal static async Task<bool> Save(GymAppContext gymAppContext) {
        return await gymAppContext.SaveChangesAsync() >= 0 ? true : false;
    }
}