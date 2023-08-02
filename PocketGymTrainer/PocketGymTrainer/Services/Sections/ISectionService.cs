using PocketGymTrainer.Models;
using ErrorOr;

namespace PocketGymTrainer.Services.Sections;

public interface ISectionService
{
    ErrorOr<Created> CreateSection(Section section);
    ErrorOr<List<Section>> GetSection();
    ErrorOr<UpsertedSection> UpsertSection(Section section);
    ErrorOr<Deleted> DeleteSection(Guid id);
    Dictionary<Guid, Section> addData(Section section);
    Dictionary<Guid, Section> addGetData();
    void removeData();
}