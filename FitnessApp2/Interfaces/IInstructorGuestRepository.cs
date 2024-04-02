using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface IInstructorGuestRepository
    {
        ICollection<InstructorGuest> GetInstructorsByGuestId(int guestId);
        ICollection<InstructorGuest> GetGuestsByInstructorId(int courseId);

        bool InstructorHasGuests(int courseId);
        bool GuestHasInstructors(int instrucId);
    }
}
