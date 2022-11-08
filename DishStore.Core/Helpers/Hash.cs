namespace DishStore.Core.Helpers;

public class Hash
{
    public static string HashString(string text, string salt = "")
    {
        if (string.IsNullOrEmpty(text))
        {
            return String.Empty;
        }

        using var sha = new System.Security.Cryptography.SHA256Managed();
        var textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
        var hashBytes = sha.ComputeHash(textBytes);

        var hash = BitConverter.ToString(hashBytes);
        return hash;
    }
}