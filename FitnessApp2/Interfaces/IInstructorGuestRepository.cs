using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface IInstructorGuestRepository
    {
        ICollection<InstructorGuest> GetInstructorsByGuestId(int guestId);
        ICollection<InstructorGuest> GetGuestsByInstructorId(int courseId);
        public InstructorGuest GetInstructorGuestByInstructorIdAndGuestId(int instrucId, int guestId);

        bool InstructorHasGuests(int courseId);
        bool GuestHasInstructors(int instrucId);

        //creating, updating
        public bool RegisterGuest(InstructorGuest instructorGuest);

        //deleting all existing instructor<->guest
        public bool DeleteAllInstructorGuest(List<InstructorGuest> listOfInstructorGuest);

        //deleting one guest from a instructor<->guest
        public bool DeleteInstructorGuest(InstructorGuest instructorGuest);

    }
}
