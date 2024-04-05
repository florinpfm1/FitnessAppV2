using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Repository
{
    public class SectionRepository : ISectionRepository
    {
        private FAppDbContext _context;

        public SectionRepository(FAppDbContext context)
        {
            this._context = context;
        }

        //get a List of all sections
        public ICollection<Section> GetSections()
        {
            return _context.Sections.OrderBy(s => s.Id).ToList();
        }

        //get one section by Id
        public Section GetSection(int? id)
        {
            return _context.Sections.Where(s => s.Id == id).FirstOrDefault();
        }

        //get one section by Name
        public Section GetSection(string name)
        {
            return _context.Sections.Where(s => s.Name == name).FirstOrDefault();
        }

        //checks if section with specified id exists in db
        public bool SectionExists(int sectionId)
        {
            return _context.Sections.Any(s => s.Id == sectionId);
        }
    }
}
