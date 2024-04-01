using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private FAppDbContext _context;

        public CourseRepository(FAppDbContext context)
        {
            this._context = context;   
        }

        //get a List of all courses
        public ICollection<Course> GetCourses()
        {
            return _context.Courses.OrderBy(c => c.Id).ToList();
        }

        //get one course by Id
        public Course GetCourse(int id)
        {
            return _context.Courses.Where(c => c.Id == id).FirstOrDefault();
        }

        //get one course by Name
        public Course GetCourse(string name)
        {
            return _context.Courses.Where(c => c.Name == name).FirstOrDefault();
        }

        //get a List of courses by Difficulty
        public ICollection<Course> GetCourses(string difficulty)
        {
            return _context.Courses.Where(c => c.Difficulty == difficulty).ToList();
        }

        //get a List of courses by Rating
        public ICollection<Course> GetCourses(byte rating)
        {
            return _context.Courses.Where(c => c.Rating == rating).ToList();
        }

        //checks if course with specified Id exists in db
        public bool CourseExists(int courseId)
        {
            return _context.Courses.Any(c => c.Id == courseId);
        }
    }
}
