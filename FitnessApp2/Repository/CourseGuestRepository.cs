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
    }
}
