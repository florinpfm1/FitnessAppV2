using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface ICourseInstructorRepository
    {
        //retrieving
        public ICollection<CourseInstructor> GetCoursesByInstructorId(int instrucId);
        public ICollection<CourseInstructor> GetCoursesNotAssignedToInstructorId(int instrucId);
        public ICollection<CourseInstructor> GetInstructorsByCourseId(int courseId);
        public CourseInstructor GetCourseInstructorByCourseIdAndInstructorId(int courseId, int instrucId);

        //checking
        public bool CourseHasInstructor(int courseId);
        public bool InstructorHasCourse(int instrucId);

        //assign an instructor to a course
        public bool AssignInstructor(CourseInstructor courseInstructor);

        //deleting all existing course<->instructor
        public bool DeleteAllCourseInstructor(List<CourseInstructor> listOfCourseInstructor);
        //deleting one instructor from a course<->instructor
        public bool DeleteCourseInstructor(CourseInstructor courseInstructor);
    }
}
