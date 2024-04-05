using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface ISectionRepository
    {
        ICollection<Section> GetSections();
        Section GetSection(int? id);
        Section GetSection(string name);
        bool SectionExists(int sectionId);
    }
}
