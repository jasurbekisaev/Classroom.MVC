namespace Classroom.MVC.Helpers;

public class FileHelper
{
    private const string wwwroot = "wwwroot";

    private static void CheckDirectory(string folder)
    {
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
    }

    public static async Task<string> SaveUserFile(IFormFile file)
    {
        return await SaveFile(file, "UserFiles");
    }
    public static async Task<string> SaveSchoolFile(IFormFile file)
    {
        return await SaveFile(file, "SchoolFiles");
    }

    public static async Task<string> SaveFile(IFormFile file, string folder)
    {
        CheckDirectory(folder);
        var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);

        var ms = new MemoryStream();

        await file.CopyToAsync(ms);
        await File.WriteAllBytesAsync(Path.Combine(wwwroot, folder, filename), ms.ToArray());

        return $"/{folder}/{filename}";

    }
}
