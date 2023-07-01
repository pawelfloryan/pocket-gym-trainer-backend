using PocketGymTrainer.Models;
using ErrorOr;

namespace PocketGymTrainer.Services.Photos;

public interface IStorageProvider
{
    void SaveFile(string fileName, byte[] fileContent);
}