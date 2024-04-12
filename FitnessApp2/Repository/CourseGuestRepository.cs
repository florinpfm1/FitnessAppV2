using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Repository
{
    public class CourseGuestRepository : ICourseGuestRepository
    {
        private readonly FAppDbContext _context;
        public CourseGuestRepository(FAppDbContext context)
        {
            this._context = context;
        }

        //retrieving
        public ICollection<CourseGuest> GetCoursesByGuestId(int guestId)
        {
            return _context.CourseGuests.Where(c => c.GuestId == guestId).ToList();
        }

        public ICollection<CourseGuest> GetCoursesNotAssignedToGuestId(int guestId)
        {
            ICollection<CourseGuest> collection = _context.CourseGuests.Where(g => g.GuestId != guestId).ToList();
            return collection.DistinctBy(g => g.CourseId).ToList();
        }

        public ICollection<CourseGuest> GetGuestsByCourseId(int courseId)
        {
            return _context.CourseGuests.Where(g => g.CourseId == courseId).ToList();
        }

        public CourseGuest GetCourseGuestByCourseIdAndGuestId(int courseId, int guestId)
        {
            return _context.CourseGuests.Where(x => x.CourseId == courseId && x.GuestId == guestId).FirstOrDefault();
        }

        //checking
        public bool CourseHasGuests(int courseId)
        {
            return _context.CourseGuests.Any(c => c.CourseId == courseId);
        }

        public bool GuestHasCourse(int guestId)
        {
            return _context.CourseGuests.Any(g => g.GuestId == guestId);
        }

        //assign a guest to a course
        public bool RegisterGuest(CourseGuest courseGuest)
        {
            _context.Add(courseGuest);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); 
            return saved > 0 ? true : false;
        }

        //deleting all existing course<->guest
        public bool DeleteAllCourseGuest(List<CourseGuest> listOfCourseGuest)
        {
            foreach (CourseGuest courseGuest in listOfCourseGuest)
            {
                _context.Remove(courseGuest);
            }
            return Save();
        }

        //deleting one guest from a course<->guest
        public bool DeleteCourseGuest(CourseGuest courseGuest)
        {
            _context.Remove(courseGuest);
            return Save();
        }
    }
}
