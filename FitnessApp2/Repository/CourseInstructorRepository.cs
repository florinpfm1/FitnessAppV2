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

        //retrieving
        public ICollection<CourseInstructor> GetCoursesByInstructorId(int instrucId)
        {
            return _context.CourseInstructors.Where(c => c.InstructorId == instrucId).ToList();
        }

        public ICollection<CourseInstructor> GetCoursesNotAssignedToInstructorId(int instrucId)
        {
            ICollection<CourseInstructor> collection = _context.CourseInstructors.Where(c => c.InstructorId != instrucId).ToList();
            return collection.DistinctBy(c => c.CourseId).ToList();
        }

        public ICollection<CourseInstructor> GetInstructorsByCourseId(int courseId)
        {
            return _context.CourseInstructors.Where(i => i.CourseId == courseId).ToList();
        }

        public CourseInstructor GetCourseInstructorByCourseIdAndInstructorId(int courseId, int instrucId) 
        {
            return _context.CourseInstructors.Where(i => i.InstructorId == instrucId && i.CourseId == courseId).FirstOrDefault();

        }

        //checking
        public bool CourseHasInstructor(int courseId)
        {
            return _context.CourseInstructors.Any(c => c.CourseId == courseId);
        }

        public bool InstructorHasCourse(int instrucId)
        {
            return _context.CourseInstructors.Any(c => c.InstructorId == instrucId);
        }


        //assign an instructor to a course
        public bool AssignInstructor(CourseInstructor courseInstructor)
        {
            _context.Add(courseInstructor);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        //deleting all existing course<->instructor
        public bool DeleteAllCourseInstructor(List<CourseInstructor> listOfCourseInstructor)
        {
            foreach (CourseInstructor courseInstructor in listOfCourseInstructor)
            {
                _context.Remove(courseInstructor);
            }
            return Save();
        }

        //deleting one instructor from a course<->instructor
        public bool DeleteCourseInstructor(CourseInstructor courseInstructor)
        {
            _context.Remove(courseInstructor);
            return Save();
        }
    }
}
