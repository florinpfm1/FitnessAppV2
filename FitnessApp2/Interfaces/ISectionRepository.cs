using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface ISectionRepository
    {
        //retrieving
        public ICollection<Section> GetSections();
        public Section GetSection(int? id);
        public Section GetSection(string name);
        //checking
        public bool SectionExists(int sectionId);
    }
}
