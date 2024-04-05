using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface IGuestRepository
    {
        //retrieving
        ICollection<Guest> GetGuests();
        ICollection<Guest> GetAssignedGuests();
        ICollection<Guest> GetUnassignedGuests();
        Guest GetGuest(int id);
        Guest GetGuest(string firstName, string lastName);
        //ICollection<Guest> GetGuests(DateOnly date);
        ICollection<Guest> GetGuests(byte hours);
        bool GuestExists(int guestId);

        //creating
        public bool CreateGuest(Guest guest);

        //updating
        public bool UpdateGuest(Guest guest);

        //deleting
        public bool DeleteGuest(Guest guest);
    }
}
