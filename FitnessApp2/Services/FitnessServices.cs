using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp2.Services
{
    public class FitnessServices : IFitnessServices
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IDetailRepository _detailRepository;
        private readonly ICourseInstructorRepository _courseInstructorRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseGuestRepository _courseGuestRepository;
        private readonly IInstructorGuestRepository _instructorGuestRepository;

        public FitnessServices(
            IInstructorRepository instructorRepository,
            IGuestRepository guestRepository,
            ISectionRepository sectionRepository,
            IDetailRepository detailRepository,
            ICourseInstructorRepository courseInstructorRepository,
            ICourseRepository courseRepository,
            ICourseGuestRepository courseGuestRepository,
            IInstructorGuestRepository instructorGuestRepository
            )
        {
            this._instructorRepository = instructorRepository;
            this._guestRepository = guestRepository;
            this._sectionRepository = sectionRepository;
            this._detailRepository = detailRepository;
            this._courseInstructorRepository = courseInstructorRepository;
            this._courseRepository = courseRepository;
            this._courseGuestRepository = courseGuestRepository;
            this._instructorGuestRepository = instructorGuestRepository;
        }

        //INSTRUCTORS
        //retrieve a list with all instructors from repository
        public ICollection<Instructor> GetInstructors()
        {
            return _instructorRepository.GetInstructors();
        }

        //retrieve one instructor by Id
        public Instructor GetInstructor(int id)
        {
            return _instructorRepository.GetInstructor(id);
        }

        //retrieve one instructor by FirstName and LastName
        public Instructor GetInstructor(string firstName, string lastName)
        {
            return _instructorRepository.GetInstructor(firstName, lastName);
        }

        //checks if an instructors exists in db with that Id
        public bool InstructorExists(int instrucId)
        {
            return _instructorRepository.InstructorExists(instrucId);
        }


        //creating a new instructor
        public bool CreateInstructor(Instructor instructor)
        {
            return _instructorRepository.CreateInstructor(instructor);
        }


        //updating an existing instructor
        public bool UpdateInstructor(Instructor instructor)
        {
            return _instructorRepository.UpdateInstructor(instructor);
        }

        //deleting an existing instructor
        public bool DeleteInstructor(Instructor instructor)
        {
            return _instructorRepository.DeleteInstructor(instructor);
        }

        //GUESTS
        //retrieve all assigned guests from repository
        public ICollection<Guest> GetAssignedGuests()
        {
            return _guestRepository.GetAssignedGuests();
        }

        //retrieve all unassigned guests from repository
        public ICollection<Guest> GetUnassignedGuests()
        {
            return _guestRepository.GetUnassignedGuests();
        }

        //get one guest by id
        public Guest GetGuest(int id)
        {
            return _guestRepository.GetGuest(id);
        }

        //get one guest by FirstName and LastName
        public Guest GetGuest(string firstName, string lastName)
        {
            return _guestRepository.GetGuest(firstName, lastName);
        }

        //get one section by Id
        public Section GetSection(int? id)
        {
            return _sectionRepository.GetSection(id);
        }

        //get one section by Name
        public Section GetSection(string name)
        {
            return _sectionRepository.GetSection(name);
        }

        //get one detail by id
        public Detail GetDetail(int? id)
        {
            return _detailRepository.GetDetail(id);
        }

        //checks if guest with the specified Id exists in db
        public bool GuestExists(int guestId)
        {
            return _guestRepository.GuestExists(guestId);
        }


        //creating a new guest
        public bool CreateGuest(Guest guest)
        {
            return _guestRepository.CreateGuest(guest);
        }

        //updating an existing instructor
        public bool UpdateGuest(Guest guest)
        {
            return _guestRepository.UpdateGuest(guest);
        }

        //deleting an existing instructor
        public bool DeleteGuest(Guest guest)
        {
            return _guestRepository.DeleteGuest(guest);
        }

        //ASSIGN INSTRUCTORS
        //retrieve a list with all instructors and their courses from repository
        public ICollection<AssignInstructorViewModel> GetAssignInstructors()
        {
            ICollection<Instructor> instructorsAsList = _instructorRepository.GetInstructors().ToList();
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

            return assignInstructorsViewModel;
        }

        //retrieve assigned courses for AssignInstructor by id
        public List<SelectListItem> GetAssignedCoursesAssignInstruc(int Id)
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

            return availableCoursesToAssign;
        }

        //check if instructor was assigned to this course
        public bool CheckInstructorAssignedToCourse(string courseChosen, int instrucId)
        {
            bool instructorAssignedToCourse = false;
            Course courseSelected = _courseRepository.GetCourse(Int32.Parse(courseChosen));
            List<CourseInstructor> coursesForInstructor = _courseInstructorRepository.GetCoursesByInstructorId(instrucId).ToList();
            foreach (CourseInstructor course in coursesForInstructor)
            {
                if (course.CourseId == courseSelected.Id)
                {
                    instructorAssignedToCourse = true;
                    break;
                }
            }
            return instructorAssignedToCourse;
        }

        //check if instructor has at least 5 hours free to take on one more guest (which can choose between 1...5 hours)
        public bool CheckInstructorHasFreeHours(int instrucId)
        {
            bool instrucHasFreeHours = false;
            bool instrucHasCourses = _courseInstructorRepository.InstructorHasCourse(instrucId);
            List<CourseInstructor> coursesForInstructor = _courseInstructorRepository.GetCoursesByInstructorId(instrucId).ToList();
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
                        ICollection<InstructorGuest> guestsForInstructor = _instructorGuestRepository.GetGuestsByInstructorId(instrucId);
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

            return instrucHasFreeHours;
        }

        //get one course by Id
        public Course GetCourse(int id)
        {
            return _courseRepository.GetCourse(id);
        }

        //get one course by Name
        public Course GetCourse(string name)
        {
            return _courseRepository.GetCourse(name);
        }


        //assigning an instructor to a course
        public bool AssignInstructor(CourseInstructor courseInstructor)
        {
            return _courseInstructorRepository.AssignInstructor(courseInstructor);
        }
    }
}
