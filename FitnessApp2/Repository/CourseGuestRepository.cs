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


        public bool CourseHasGuests(int courseId)
        {
            return _context.CourseGuests.Any(c => c.CourseId == courseId);
        }

        public bool GuestHasCourse(int guestId)
        {
            return _context.CourseGuests.Any(g => g.GuestId == guestId);
        }

        //assign a guest to a course
        public bool AssignGuest(CourseGuest courseGuest)
        {
            _context.Add(courseGuest);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); 
            return saved > 0 ? true : false;
        }
    }
}
