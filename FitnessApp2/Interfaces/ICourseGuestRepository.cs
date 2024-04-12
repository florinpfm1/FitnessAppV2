using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface ICourseGuestRepository
    {
        ICollection<CourseGuest> GetCoursesByGuestId(int guestId);
        ICollection<CourseGuest> GetCoursesNotAssignedToGuestId(int guestId);
        ICollection<CourseGuest> GetGuestsByCourseId(int courseId);
        public CourseGuest GetCourseGuestByCourseIdAndGuestId(int courseId, int guestId);

        bool CourseHasGuests(int courseId);
        bool GuestHasCourse(int instrucId);

        //creating, updating
        bool RegisterGuest(CourseGuest courseGuest);

        //deleting all existing course<->guest
        public bool DeleteAllCourseGuest(List<CourseGuest> listOfCourseGuest);

        //deleting one guest from a course<->guest
        public bool DeleteCourseGuest(CourseGuest courseGuest);
    }
}
