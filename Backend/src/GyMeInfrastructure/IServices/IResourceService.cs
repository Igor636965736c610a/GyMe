using Microsoft.AspNetCore.Http;

namespace GyMeInfrastructure.IServices;

internal interface IGyMeResourceService
{
    string GenerateUrlToPhoto(string fileName, string userId);
    string GeneratePathToUserResourceFolder(string userId);
    string GeneratePathToPhoto(string fileName, string userId);
    void RemoveProfilePicture(string userId);
    void SetDefaultProfilePicture(string userId);
    Task SaveImageOnServer(IFormFile? image, string path);
}