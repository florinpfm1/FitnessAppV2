using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface ICourseRepository
    {
        ICollection<Course> GetCourses();
        Course GetCourse(int id);
        Course GetCourse(string name);
        ICollection<Course> GetCourses(string difficulty);
        ICollection<Course> GetCourses(byte rating);
        bool CourseExists(int courseId);
    }
}
