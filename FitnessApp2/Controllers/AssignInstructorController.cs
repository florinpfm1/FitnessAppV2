using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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

        //--------------- RETRIEVE ALL INSTRUCTORS AND THEIR COURSES ---------------
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

        //--------------- ASSIGN INSTRUCTOR TO COURSE ---------------
        [HttpGet]
        public IActionResult AssignInstructor(int Id)
        {
            //get a list of course that can be assigned to instructor
            List<Course> allCourses = _courseRepository.GetCourses().ToList();
            List<CourseInstructor> coursesForInstructor = _courseInstructorRepository.GetCoursesNotAssignedToInstructorId(Id).ToList();

            List<Course> availableCourses = new List<Course>();

            if (coursesForInstructor.Count != 0)
            {
                foreach (CourseInstructor courseForInstructor in coursesForInstructor)
                {
                    Course currentCourse = _courseRepository.GetCourse(courseForInstructor.CourseId);
                    availableCourses.Add(currentCourse);
                }
            }

            var availableCoursesToAssign = (coursesForInstructor.Count != 0) ? availableCourses : allCourses;

            ViewData["availableCoursesToAssign"] = availableCoursesToAssign;

            try
            {
                Instructor instructor = _instructorRepository.GetInstructor(Id);

                if (instructor != null) 
                {
                    AssignInstructorViewModel assignInstructorViewModel = new AssignInstructorViewModel()
                    {
                        Id = instructor.Id,
                        FirstName = instructor.FirstName,
                        LastName = instructor.LastName,
                    };
                    return View(assignInstructorViewModel);
                }
                else
                {
                    TempData["errorMessage"] = $"Instructor with the InstructorID: {Id} is not found."; //aici {Id} e cel ca param de intrare "int Id" la actiunea Edit
                    return RedirectToAction("GetInstructorsAndCourses", "AssignInstructor");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetInstructorsAndCourses", "AssignInstructor");
            }
            
        }

        [HttpPost]
        public IActionResult AssignInstructor(AssignInstructorViewModel assignInstructorViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Course courseSelected = _courseRepository.GetCourse(assignInstructorViewModel.CourseName);
                    bool instructorAlreadyAssigned = false;
                    var coursesOfInstructor = _courseInstructorRepository.GetCoursesByInstructorId(assignInstructorViewModel.Id);
                    foreach (var course in coursesOfInstructor)
                    {
                        if(course.CourseId == courseSelected.Id)
                        {
                            instructorAlreadyAssigned = true;
                            break;
                        }
                    }

                    if (!instructorAlreadyAssigned) 
                    {
                        //to add here logic
                        bool instrucHasFreeHours = true;
                        if (instrucHasFreeHours)
                        {
                            CourseInstructor courseInstructor = new CourseInstructor()
                            {
                                CourseId = courseSelected.Id,
                                InstructorId = assignInstructorViewModel.Id
                            };

                            bool statusAssignInstructorInDb = _courseInstructorRepository.AssignInstructor(courseInstructor);
                            if (statusAssignInstructorInDb)
                            {
                                TempData["successMessage"] = "Instructor assigned successfully!";
                                return RedirectToAction("GetInstructorsAndCourses", "AssignInstructor");
                            }
                            else
                            {
                                TempData["errorMessage"] = "Something went wrong when saving to database.";
                                return View();
                            }
                        } 
                        else
                        {
                            TempData["errorMessage"] = "Instructor does not have free hours to start a new course.";
                            return View();
                        }
                        
                    }
                    else
                    {
                        TempData["errorMessage"] = "Instructor is already assigned to this course.";
                        return View();
                    }

                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

         
        }

    }
}
