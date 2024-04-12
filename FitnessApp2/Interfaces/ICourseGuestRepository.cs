using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface ICourseGuestRepository
    {
        //retrieving
        public ICollection<CourseGuest> GetCoursesByGuestId(int guestId);
        public ICollection<CourseGuest> GetCoursesNotAssignedToGuestId(int guestId);
        public ICollection<CourseGuest> GetGuestsByCourseId(int courseId);
        public CourseGuest GetCourseGuestByCourseIdAndGuestId(int courseId, int guestId);

        //checking
        public bool CourseHasGuests(int courseId);
        public bool GuestHasCourse(int instrucId);

        //creating, updating
        public bool RegisterGuest(CourseGuest courseGuest);

        //deleting all existing course<->guest
        public bool DeleteAllCourseGuest(List<CourseGuest> listOfCourseGuest);

        //deleting one guest from a course<->guest
        public bool DeleteCourseGuest(CourseGuest courseGuest);
    }
}
