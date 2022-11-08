namespace DishStore.Helpers;

public class ImageHelper
{
    public static async Task SaveImage(IFormFile image, string path)
    {
        await using var fileStream = new FileStream(path, FileMode.Create);
        await image.CopyToAsync(fileStream);
    }
}