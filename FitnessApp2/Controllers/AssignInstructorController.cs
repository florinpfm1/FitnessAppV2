using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace FitnessApp2.Controllers
{
    public class AssignInstructorController : Controller
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICourseInstructorRepository _courseInstructorRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseGuestRepository _courseGuestRepository;
        private readonly IInstructorGuestRepository _instructorGuestRepository;
        private readonly IGuestRepository _guestRepository;


        public AssignInstructorController(
            IInstructorRepository instructorRepository, 
            ICourseInstructorRepository courseInstructorRepository,
            ICourseRepository courseRepository,
            ICourseGuestRepository courseGuestRepository,
            IInstructorGuestRepository instructorGuestRepository,
            IGuestRepository guestRepository
            )
        {
            this._instructorRepository = instructorRepository;
            this._courseInstructorRepository = courseInstructorRepository;
            this._courseRepository = courseRepository;
            this._courseGuestRepository = courseGuestRepository;
            this._instructorGuestRepository = instructorGuestRepository;
            this._guestRepository = guestRepository;
        }

        //--------------- RETRIEVE ALL INSTRUCTORS AND THEIR COURSES ---------------
        [HttpGet]
        public IActionResult GetAssignInstructors()
        {
            List<Instructor> instructorsAsList = _instructorRepository.GetInstructors().ToList();
            List<AssignInstructorViewModel> assignInstructorsViewModel = new List<AssignInstructorViewModel>();

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

                AssignInstructorViewModel assignInstructorViewModel = new AssignInstructorViewModel()
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    AssignedCourses = (instrucHasCourses) ? courseNamesAsListOfStrings : null,
                };
                assignInstructorsViewModel.Add(assignInstructorViewModel);
            }
            return View(assignInstructorsViewModel);
        }

        //--------------- ASSIGN INSTRUCTOR TO COURSE ---------------
        [HttpGet]
        public IActionResult EditAssignInstructor(int Id)
        {
            //get a list of course that can be assigned to instructor
            List<Course> allCoursesInDb = _courseRepository.GetCourses().ToList();
            List<CourseInstructor> coursesForInstructor = _courseInstructorRepository.GetCoursesByInstructorId(Id).ToList();
            List<Course> availableCourses = _courseRepository.GetCourses().ToList();

            if (coursesForInstructor.Count != 0)
            {
                foreach (CourseInstructor courseForInstructor in coursesForInstructor)
                {
                    foreach (Course course in allCoursesInDb)
                    {
                        if (courseForInstructor.CourseId == course.Id)
                        {
                            availableCourses.Remove(course);
                            break;
                        }
                    }
                }
            }

            List<Course> filteredCourses = (coursesForInstructor.Count != 0) ? availableCourses : allCoursesInDb;

            List<SelectListItem> availableCoursesToAssign = new List<SelectListItem>();
            foreach (Course course in filteredCourses)
            {
                availableCoursesToAssign.Add(new SelectListItem { Text = course.Name, Value = course.Id.ToString() });
            }

            //retrieve instructor info from db and prepare AssignInstructorViewModel for POST below
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
                        AvailableCoursesToAssign = availableCoursesToAssign, //list of strings
                    };
                    return View(assignInstructorViewModel);
                }
                else
                {
                    TempData["errorMessage"] = $"Instructor with the InstructorID: {Id} is not found."; //aici {Id} e cel ca param de intrare "int Id" la actiunea Edit
                    return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetAssignInstructors", "AssignInstructor");
            }
            
        }

        [HttpPost]
        public IActionResult EditAssignInstructor(AssignInstructorViewModel assignInstructorViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check if instructor was assigned to this course
                    bool instructorAssignedToCourse = false;
                    Course courseSelected = _courseRepository.GetCourse(Int32.Parse(assignInstructorViewModel.CourseSelected));
                    List<CourseInstructor> coursesForInstructor = _courseInstructorRepository.GetCoursesByInstructorId(assignInstructorViewModel.Id).ToList();
                    foreach (CourseInstructor course in coursesForInstructor)
                    {
                        if(course.CourseId == courseSelected.Id)
                        {
                            instructorAssignedToCourse = true;
                            break;
                        }
                    }

                    if (!instructorAssignedToCourse) 
                    {
                        //check if instructor has at least 5 hours free to take on one more guest (which can choose between 1...5 hours)
                        bool instrucHasFreeHours = false;
                        bool instrucHasCourses = _courseInstructorRepository.InstructorHasCourse(assignInstructorViewModel.Id);
                        byte assignedHoursForInstructor = (byte)0; //initialize instructor assigned hours from all guests

                        if (instrucHasCourses)
                        {
                            foreach (CourseInstructor courseForInstructor in coursesForInstructor)
                            {
                                Course currentCourse = new Course();
                                currentCourse = _courseRepository.GetCourse(courseForInstructor.CourseId);

                                bool courseHasGuests = _courseGuestRepository.CourseHasGuests(courseForInstructor.CourseId);

                                if (courseHasGuests)
                                {
                                    ICollection<CourseGuest> guestsForCourse = _courseGuestRepository.GetGuestsByCourseId(courseForInstructor.CourseId);
                                    ICollection<InstructorGuest> guestsForInstructor = _instructorGuestRepository.GetGuestsByInstructorId(assignInstructorViewModel.Id);
                                    foreach (CourseGuest guestForCourse in guestsForCourse)
                                    {
                                        foreach (InstructorGuest guestForInstructor in guestsForInstructor)
                                        {
                                            if (guestForInstructor.GuestId == guestForCourse.GuestId)
                                            { 
                                                Guest currentGuest = _guestRepository.GetGuest(guestForCourse.GuestId);
                                                assignedHoursForInstructor += currentGuest.Hours; //add hours of each guestOfInstructor to his instructor
                                            }
                                        }

                                    }
                                }
                            }
                        }

                        instrucHasFreeHours = (assignedHoursForInstructor <= (byte)35) ? true : false;

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
                                return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                            }
                            else
                            {
                                TempData["errorMessage"] = "Something went wrong when saving to database.";
                                return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                            }
                        } 
                        else
                        {
                            TempData["errorMessage"] = "Instructor does not have free hours to start a new course.";
                            return View(assignInstructorViewModel);
                        }
                        
                    }
                    else
                    {
                        TempData["errorMessage"] = "Instructor is already assigned to this course.";
                        return View(assignInstructorViewModel);
                    }

                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View(assignInstructorViewModel);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetAssignInstructors", "AssignInstructor");
            }

         
        }

    }
}
