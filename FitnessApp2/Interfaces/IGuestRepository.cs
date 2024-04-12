using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface IGuestRepository
    {
        //retrieving
        public ICollection<Guest> GetGuests();
        public ICollection<Guest> GetAssignedGuests();
        public ICollection<Guest> GetUnassignedGuests();
        public Guest GetGuest(int id);
        public Guest GetGuest(string firstName, string lastName);
        public ICollection<Guest> GetGuests(byte hours);
        //checking
        public bool GuestExists(int guestId);

        //creating
        public bool CreateGuest(Guest guest);

        //updating
        public bool UpdateGuest(Guest guest);

        //deleting
        public bool DeleteGuest(Guest guest);
    }
}
