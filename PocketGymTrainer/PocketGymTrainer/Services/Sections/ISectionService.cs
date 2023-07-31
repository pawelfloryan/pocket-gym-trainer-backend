using PocketGymTrainer.Models;
using ErrorOr;

namespace PocketGymTrainer.Services.Sections;

public interface ISectionService
{
    ErrorOr<Section> CreateSection(Section section);
    ErrorOr<List<Section>> GetSection();
    ErrorOr<UpsertedSection> UpsertSection(Section section);
    ErrorOr<Deleted> DeleteSection(Guid id);
    void removeData();
}