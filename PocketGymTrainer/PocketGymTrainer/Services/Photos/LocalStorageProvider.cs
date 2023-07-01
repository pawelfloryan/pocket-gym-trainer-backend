using ErrorOr;
using PocketGymTrainer.Services.Photos;

namespace PocketGymTrainer.Services.Photos;

public class StorageProvider : IStorageProvider
{
    public void SaveFile(string fileName, byte[] fileContent)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Static", fileName);
        File.WriteAllBytes(filePath, fileContent);
    }
}