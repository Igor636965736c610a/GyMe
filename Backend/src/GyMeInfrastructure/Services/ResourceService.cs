using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.Services;

internal class GyMeResourceService : IGyMeResourceService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GyMeResourceService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateUrlToPhoto(string fileName, string userId)
    {
        var photoUrl = Path.Combine(GeneratePathToUserResourceFolder(userId), fileName);

        return photoUrl;
    }
    
    public string GeneratePathToUserResourceFolder(string userId)
    {
        var path = Path.Combine($"https://{_httpContextAccessor.HttpContext!.Request.Host}", "Images", userId);

        return path;
    }

    public string GeneratePathToPhoto(string fileName, string userId)
    {
        var photoPath = Path.Combine(_webHostEnvironment.WebRootPath, @$"Images\{userId}\{fileName}");

        return photoPath;
    }
    
    public void SetDefaultProfilePicture(string userId)
    {
        var userDirectory = Path.Combine(_webHostEnvironment.WebRootPath, @$"Images\{userId}");
        Directory.CreateDirectory(userDirectory);
        var defaultPhotoPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", "defaultProfilePicture.jpg");
        var destinationPath = Path.Combine(userDirectory, userId + ".jpg");
        File.Copy(defaultPhotoPath, destinationPath, overwrite:true);
    }

    public void RemoveProfilePicture(string userId)
    {
        var userDirectory = Path.Combine(_webHostEnvironment.WebRootPath, @$"Images\{userId}");
     
        var fileToDelete = Directory.GetFiles(userDirectory, userId + ".*").FirstOrDefault();
        if (fileToDelete is not null)
        {
            File.Delete(fileToDelete);
        }
    }
    
    public async Task SaveImageOnServer(IFormFile? image, string path)
    {
        if (image is null)
        {
            throw new InvalidOperationException("Empty Image");
        }
            
        Directory.CreateDirectory(path);
        await using var stream = new FileStream(path, FileMode.Create);
        await image.CopyToAsync(stream);
    }
}