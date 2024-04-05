using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Repository
{
    public class GuestRepository : IGuestRepository
    {
        private FAppDbContext _context;

        public GuestRepository(FAppDbContext context)
        {
            this._context=context;
        }

        //get a List of all guests
        public ICollection<Guest> GetGuests()
        {
            return _context.Guests.OrderBy(g => g.Id).ToList();
        }

        //get a List of all assigned guests
        public ICollection<Guest> GetAssignedGuests()
        {
            return _context.Guests.Where(g => g.DetailId == null).OrderBy(g => g.Id).ToList();
        }

        //get a List of all unassigned guests
        public ICollection<Guest> GetUnassignedGuests()
        {
            return _context.Guests.Where(g => g.DetailId != null).OrderBy(g => g.Id).ToList();
        }

        //get one guest by id
        public Guest GetGuest(int id)
        {
            return _context.Guests.Where(g => g.Id == id).FirstOrDefault();
        }

        //get one guest by FirstName and LastName
        public Guest GetGuest(string firstName, string lastName)
        {
            return _context.Guests.Where(g => g.FirstName == firstName && g.LastName == lastName).FirstOrDefault();
        }

        public ICollection<Guest> GetGuests(DateOnly date)
        {
            throw new NotImplementedException();
        }

        //get a List of guests by Hours
        public ICollection<Guest> GetGuests(byte hours)
        {
            return _context.Guests.Where(g => g.Hours == hours).ToList();
        }

        //checks if guest with the specified Id exists in db
        public bool GuestExists(int guestId)
        {
            return _context.Guests.Any(g => g.Id == guestId);
        }

        //creating a new guest
        public bool CreateGuest(Guest guest)
        {
            _context.Add(guest);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        //updating an existing guest
        public bool UpdateGuest(Guest guest)
        {
            _context.Update(guest);
            return Save();
        }

        //deleting an existing guest
        public bool DeleteGuest(Guest guest)
        {
            _context.Remove(guest);
            return Save();
        }
    }
}
