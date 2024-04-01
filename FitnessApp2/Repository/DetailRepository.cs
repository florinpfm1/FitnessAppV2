using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Repository
{
    public class DetailRepository : IDetailRepository
    {
        private FAppDbContext _context;

        public DetailRepository(FAppDbContext context)
        {
            this._context = context;
        }

        //get a List of all details
        public ICollection<Detail> GetDetails()
        {
            return _context.Details.OrderBy(d  => d.Id).ToList();
        }

        //get one detail by id
        public Detail GetDetail(int? id)
        {
            return _context.Details.Where(d => d.Id == id).FirstOrDefault();
        }

        //get one detail by email
        public Detail GetDetailByEmail(string email)
        {
            return _context.Details.Where(d => d.Email == email).FirstOrDefault();
        }

        //get one detail by phone
        public Detail GetDetailByPhone(string phone)
        {
            return _context.Details.Where(d => d.Phone == phone).FirstOrDefault();
        }

        //checks if detail with specified id exists in db
        public bool DetailExists(int? detailId)
        {
            return _context.Details.Any(d => d.Id == detailId);
        }
    }
}
