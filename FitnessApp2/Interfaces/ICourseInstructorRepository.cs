using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface ICourseInstructorRepository
    {
        ICollection<CourseInstructor> GetCoursesByInstructorId(int instrucId);
        ICollection<CourseInstructor> GetCoursesNotAssignedToInstructorId(int instrucId);
        ICollection<CourseInstructor> GetInstructorsByCourseId(int courseId);

        bool CourseHasInstructor(int courseId);
        bool InstructorHasCourse(int instrucId);

        //assign an instructor to a course
        bool AssignInstructor(CourseInstructor courseInstructor);
        
    }
}
