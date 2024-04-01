using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface ICourseGuestRepository
    {
        CourseGuest GetCourseByGuestId(int guestId);
        ICollection<CourseGuest> GetGuestsByCourseId(int courseId);

        bool CourseHasGuests(int courseId);
        bool GuestHasCourse(int instrucId);
    }
}
