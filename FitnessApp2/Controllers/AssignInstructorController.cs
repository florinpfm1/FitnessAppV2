using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp2.Controllers
{
    public class AssignInstructorController : Controller
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICourseInstructorRepository _courseInstructorRepository;
        private readonly ICourseRepository _courseRepository;


        public AssignInstructorController(
            IInstructorRepository instructorRepository, 
            ICourseInstructorRepository courseInstructorRepository,
            ICourseRepository courseRepository
            )
        {
            this._instructorRepository = instructorRepository;
            this._courseInstructorRepository = courseInstructorRepository;
            this._courseRepository = courseRepository;
        }

        //--------------- RETRIEVE ALL INSTRUCTORS AND COURSES ---------------
        [HttpGet]
        public IActionResult GetInstructorsAndCourses()
        {
            ICollection<Instructor> instructors = _instructorRepository.GetInstructors();
            List<Instructor> instructorsAsList = instructors.ToList();

            List<InstructorWithCoursesViewModel> instructorsWithCoursesViewModel = new List<InstructorWithCoursesViewModel>();

            foreach (Instructor instructor in instructorsAsList)
            {
                bool instrucHasCourses = _courseInstructorRepository.InstructorHasCourse(instructor.Id);
                List<string> courseNamesAsListOfStrings = new List<string> { };
                if (instrucHasCourses)
                {
                    ICollection<CourseInstructor> coursesForInstructor = _courseInstructorRepository.GetCoursesByInstructorId(instructor.Id);
                    
                    foreach (var courseForInstructor in coursesForInstructor) 
                    {
                        string courseName = _courseRepository.GetCourse(courseForInstructor.CourseId).Name;
                        courseNamesAsListOfStrings.Add(courseName);
                    }
                }

                InstructorWithCoursesViewModel instructorWithCoursesViewModel = new InstructorWithCoursesViewModel()
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    AssignedCourses = (instrucHasCourses) ? courseNamesAsListOfStrings : null,
                };
                instructorsWithCoursesViewModel.Add(instructorWithCoursesViewModel);
            }
            return View(instructorsWithCoursesViewModel);
        }

        
    }
}
