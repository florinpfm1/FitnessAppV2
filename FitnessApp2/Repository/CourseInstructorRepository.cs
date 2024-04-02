using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using static System.Collections.Specialized.BitVector32;

namespace FitnessApp2.Repository
{
    public class CourseInstructorRepository : ICourseInstructorRepository
    {
        private readonly FAppDbContext _context;

        public CourseInstructorRepository(FAppDbContext context)
        {
            this._context = context;
        }
        public ICollection<CourseInstructor> GetCoursesByInstructorId(int instrucId)
        {
            return _context.CourseInstructors.Where(c => c.InstructorId == instrucId).ToList();
        }

        public ICollection<CourseInstructor> GetCoursesNotAssignedToInstructorId(int instrucId)
        {
            ICollection<CourseInstructor> aaa = _context.CourseInstructors.Where(c => c.InstructorId != instrucId).ToList();
            return aaa.DistinctBy(c => c.CourseId).ToList();
        }

        public ICollection<CourseInstructor> GetInstructorsByCourseId(int courseId)
        {
            return _context.CourseInstructors.Where(i => i.CourseId == courseId).ToList();
        }

        public bool CourseHasInstructor(int courseId)
        {
            return _context.CourseInstructors.Any(c => c.CourseId == courseId);
        }

        public bool InstructorHasCourse(int instrucId)
        {
            return _context.CourseInstructors.Any(c => c.InstructorId == instrucId);
        }


        //assign a instructor to a course
        public bool AssignInstructor(CourseInstructor courseInstructor)
        {
            //change tracker
            //add, updating, modifying
            //connected vs disconnected   (90% we will work in connected state)
            //EntityState.Added = ...     (is a disconnected state)
            _context.Add(courseInstructor);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); //SaveChanges() returns an integer ; is the equivalent of _context.SaveChanges() put inside the CreateInstructor(...) method
            return saved > 0 ? true : false;

            //when we call SaveChanges() then Entity framework will take all we placed in _context, convert it into SQL and send it to db
        }
    }
}
