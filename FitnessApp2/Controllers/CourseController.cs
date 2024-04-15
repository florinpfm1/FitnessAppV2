using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp2.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        public CourseController(ICourseRepository courseRepository)
        {
            this._courseRepository = courseRepository;
        }

        //--------------- RETRIEVE ALL COURSES ---------------
        [HttpGet]
        public IActionResult GetCourses()
        {
            ICollection<Course> courses = _courseRepository.GetCourses();
            List<Course> coursesAsList = courses.ToList();
            List<CourseViewModel> coursesViewModel = new List<CourseViewModel>();

            foreach (Course course in coursesAsList)
            {
                CourseViewModel courseViewModel = new CourseViewModel()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = (course.Description is not null) ? course.Description : "No description available.",
                    Difficulty = (course.Difficulty is not null) ? course.Description : "No difficulty available.",
                    Rating = (course.Rating is not null) ? (byte)course.Rating : (byte)0
                };
                coursesViewModel.Add(courseViewModel);
            }
            return View(coursesViewModel);
        }
    }
}
