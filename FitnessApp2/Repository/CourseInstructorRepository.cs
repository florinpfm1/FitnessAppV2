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
    }
}
