using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.IServices;

public interface IGyMeResourceService
{
    string GeneratePathToPhoto(string fileName, string userId);
    string GeneratePathToUserResourceFolder(string userId);
    void SetDefaultProfilePicture(string userId);
    Task SaveImageOnServer(IFormFile? image, string path);
}