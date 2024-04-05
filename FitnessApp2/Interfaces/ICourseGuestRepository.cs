using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface ICourseGuestRepository
    {
        ICollection<CourseGuest> GetCoursesByGuestId(int guestId);
        ICollection<CourseGuest> GetCoursesNotAssignedToGuestId(int guestId);
        ICollection<CourseGuest> GetGuestsByCourseId(int courseId);

        bool CourseHasGuests(int courseId);
        bool GuestHasCourse(int instrucId);

        //creating, updating
        bool RegisterGuest(CourseGuest courseGuest);
       
    }
}
