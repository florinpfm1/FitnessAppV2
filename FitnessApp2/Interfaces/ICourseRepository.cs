using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface ICourseRepository
    {
        //retrieving
        public ICollection<Course> GetCourses();
        public Course GetCourse(int id);
        public Course GetCourse(string name);
        public ICollection<Course> GetCourses(string difficulty);
        public ICollection<Course> GetCourses(byte rating);
        //checking
        public bool CourseExists(int courseId);
    }
}
