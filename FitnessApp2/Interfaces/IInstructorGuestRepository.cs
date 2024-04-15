using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface IInstructorGuestRepository
    {
        //retrieving
        public ICollection<InstructorGuest> GetInstructorsByGuestId(int guestId);
        public ICollection<InstructorGuest> GetGuestsByInstructorId(int courseId);
        public InstructorGuest GetInstructorGuestByInstructorIdAndGuestId(int instrucId, int guestId);

        //checking
        public bool GuestHasInstructors(int instrucId);
        public bool InstructorHasGuests(int guestId);

        //creating, updating
        public bool RegisterGuest(InstructorGuest instructorGuest);

        //deleting all existing instructor<->guest
        public bool DeleteAllInstructorGuest(List<InstructorGuest> listOfInstructorGuest);

        //deleting one guest from a instructor<->guest
        public bool DeleteInstructorGuest(InstructorGuest instructorGuest);
    }
}
