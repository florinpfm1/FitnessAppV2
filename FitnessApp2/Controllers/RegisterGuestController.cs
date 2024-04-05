using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessApp2.Controllers
{
    public class RegisterGuestController : Controller
    {
        private readonly IGuestRepository _guestRepository;
        private readonly ICourseGuestRepository _courseGuestRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICourseInstructorRepository _courseInstructorRepository;
        private readonly IInstructorGuestRepository _instructorGuestRepository;

        public RegisterGuestController(
            IGuestRepository guestRepository,
            ICourseGuestRepository courseGuestRepository,
            ICourseRepository courseRepository,
            IInstructorRepository instructorRepository,
            ICourseInstructorRepository courseInstructorRepository,
            IInstructorGuestRepository instructorGuestRepository
            )
        {
            this._guestRepository = guestRepository;
            this._courseGuestRepository = courseGuestRepository;
            this._courseRepository = courseRepository;
            this._instructorRepository = instructorRepository;
            this._courseInstructorRepository = courseInstructorRepository;
            this._instructorGuestRepository = instructorGuestRepository;
        }

        //--------------- RETRIEVE ALL GUESTS AND THEIR COURSES ---------------
        [HttpGet]
        public IActionResult GetRegisterGuests()
        {
            List<Guest> guestsAsList = _guestRepository.GetGuests().ToList();
            List<RegisterGuestViewModel> registerGuestsViewModel = new List<RegisterGuestViewModel>();

            foreach (Guest guest in guestsAsList)
            {
                bool guestHasCourses = _courseGuestRepository.GuestHasCourse(guest.Id);
                List<string> courseNamesAsListOfStrings = new List<string>();
                if (guestHasCourses)
                {
                    ICollection<CourseGuest> coursesForGuest = _courseGuestRepository.GetCoursesByGuestId(guest.Id);
                    foreach (var courseForGuest in coursesForGuest)
                    {
                        string courseName = _courseRepository.GetCourse(courseForGuest.CourseId).Name;
                        courseNamesAsListOfStrings.Add(courseName);
                    }
                }

                RegisterGuestViewModel registerGuestViewModel = new RegisterGuestViewModel()
                {
                    Id = guest.Id,
                    FirstName = guest.FirstName,
                    LastName = guest.LastName,
                    AssignedCourses = (guestHasCourses) ? courseNamesAsListOfStrings : null,
                };
                registerGuestsViewModel.Add(registerGuestViewModel);
            }
            return View(registerGuestsViewModel);
        }

        //--------------- ASSIGN GUEST TO COURSE ---------------
        [HttpGet]
        public IActionResult EditRegisterGuest(int Id)
        {
            //get a list of courses that can be assigned to guest
            List<Course> allCoursesInDb = _courseRepository.GetCourses().ToList();
            List<CourseGuest> coursesForGuest = _courseGuestRepository.GetCoursesByGuestId(Id).ToList();
            List<Course> availableCourses = _courseRepository.GetCourses().ToList();

            if (coursesForGuest.Count != 0)
            {
                foreach (CourseGuest courseForGuest in coursesForGuest)
                {
                    foreach (Course course in allCoursesInDb)
                    {
                        if (courseForGuest.CourseId == course.Id)
                        {
                            availableCourses.Remove(course);
                            break;
                        }
                    }
                }
            }

            List<Course> filteredCourses = (coursesForGuest.Count != 0) ? availableCourses : allCoursesInDb;

            List<SelectListItem> availableCoursesToAssign = new List<SelectListItem>();
            foreach (Course course in filteredCourses)
            {
                availableCoursesToAssign.Add(new SelectListItem { Text = course.Name, Value = course.Id.ToString() });
            }

            //get a list of all instructors
            List<Instructor> allInstructorsFromDb = _instructorRepository.GetInstructors().ToList();
            List<SelectListItem> allInstructors = new List<SelectListItem>();
            foreach (Instructor instruc in allInstructorsFromDb)
            {
                allInstructors.Add(new SelectListItem { Text = instruc.FirstName + ' ' + instruc.LastName, Value = instruc.Id.ToString() });
            }


            //retrieve guest info from db and prepare RegisterGuestViewModel for POST below
            try
            {
                Guest guest = _guestRepository.GetGuest(Id);
                if (guest != null)
                {
                    RegisterGuestViewModel registerGuestViewModel = new RegisterGuestViewModel()
                    {
                        Id = guest.Id,
                        FirstName = guest.FirstName,
                        LastName = guest.LastName,
                        Hours = guest.Hours,
                        AvailableCoursesToAssign = availableCoursesToAssign, //list of strings
                        AllInstructors = allInstructors //list of strings (fName + ' ' + lName)
                    };
                    return View(registerGuestViewModel);
                }
                else
                {
                    TempData["errorMessage"] = $"Guest with the GuestID: {Id} is not found.";
                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetRegisterGuests", "RegisterGuest");
            }
        }

        [HttpPost]
        public IActionResult EditRegisterGuest(RegisterGuestViewModel registerGuestViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check if instructor was assigned to this course
                    bool instructorAssignedToCourse = false;
                    Course courseSelected = _courseRepository.GetCourse(Int32.Parse(registerGuestViewModel.CourseSelected));
                    Instructor instructorSelected = _instructorRepository.GetInstructor(Int32.Parse(registerGuestViewModel.InstructorSelected));
                    List<CourseInstructor> coursesForInstructor = _courseInstructorRepository.GetCoursesByInstructorId(instructorSelected.Id).ToList();
                    foreach (CourseInstructor course in coursesForInstructor)
                    {
                        if (course.CourseId == courseSelected.Id)
                        {
                            instructorAssignedToCourse = true;
                            break;
                        }
                    }

                    if (instructorAssignedToCourse)
                    {
                        //check if guest was already assigned to this course and to this instructor
                        bool guestAlreadyAssignedToCourse = false;
                        List<CourseGuest> coursesOfGuest = _courseGuestRepository.GetCoursesByGuestId(registerGuestViewModel.Id).ToList();
                        foreach (CourseGuest course in coursesOfGuest)
                        {
                            if (course.CourseId == courseSelected.Id)
                            {
                                guestAlreadyAssignedToCourse = true;
                                break;
                            }
                        }

                        bool guestAlreadyAssignedToInstructor = false;
                        List<InstructorGuest> guestsOfInstructor = _instructorGuestRepository.GetGuestsByInstructorId(instructorSelected.Id).ToList();
                        foreach (InstructorGuest guest in guestsOfInstructor)
                        {
                            if (guest.GuestId == registerGuestViewModel.Id)
                            {
                                guestAlreadyAssignedToInstructor = true;
                                break;
                            }
                        }

                        if (!(guestAlreadyAssignedToCourse && guestAlreadyAssignedToInstructor))
                        {
                            //check if instructor has free hours equal to the hours demanded by the guest
                            //same logic to find out hours already reserved for instructor chosen - like in Controller AssignInstructor
                            bool instrucHasFreeHours = false;
                            bool instrucHasCourses = _courseInstructorRepository.InstructorHasCourse(instructorSelected.Id);
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
                                        ICollection<InstructorGuest> guestsForInstructor = _instructorGuestRepository.GetGuestsByInstructorId(instructorSelected.Id);
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

                            instrucHasFreeHours = (assignedHoursForInstructor + registerGuestViewModel.Hours <= (byte)40) ? true : false;

                            if (instrucHasFreeHours)
                            {
                                //for assigning guest to the course
                                CourseGuest courseGuest = new CourseGuest()
                                {
                                    CourseId = courseSelected.Id,
                                    GuestId = registerGuestViewModel.Id
                                };

                                //for assigning guest to the instructor
                                InstructorGuest instructorGuest = new InstructorGuest()
                                {
                                    InstructorId = instructorSelected.Id,
                                    GuestId = registerGuestViewModel.Id
                                };

                                bool statusAssignGuestToCourseInDb = _courseGuestRepository.AssignGuest(courseGuest);
                                bool statusAssignGuestToInstructorInDb = _instructorGuestRepository.LinkInstructorAndGuest(instructorGuest);
                                if (statusAssignGuestToCourseInDb && statusAssignGuestToInstructorInDb)
                                {
                                    TempData["successMessage"] = "Guest assigned successfully!";
                                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                                }
                                else
                                {
                                    TempData["errorMessage"] = $"Something went wrong when saving to database. Guest assigned to course has status {statusAssignGuestToCourseInDb} and guest linked to instructor has status {statusAssignGuestToInstructorInDb}";
                                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                                }
                            }
                            else
                            {
                                TempData["errorMessage"] = "Instructor does not have enough free hours to take the guest.";
                                return View("EditRegisterGuest", registerGuestViewModel);
                            }

                            
                        }
                        else
                        {
                            TempData["errorMessage"] = "Guest is already assigned to this course.";
                            return View(registerGuestViewModel);
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Chosen Instructor is not assigned to this course.";
                        return View(registerGuestViewModel);
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View(registerGuestViewModel);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetRegisterGuests", "RegisterGuest");
            }
        }






    }
}
