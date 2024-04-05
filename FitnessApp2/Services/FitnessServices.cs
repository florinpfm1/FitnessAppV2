﻿using FitnessApp2.Interfaces;
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
                    foreach (CourseInstructor courseForInstructor in coursesForInstructor)
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

        //retrieve available courses to be assigned for AssignInstructor by id as <SelectListItem>
        public List<SelectListItem> GetAvailableCoursesAssignInstruc(int Id)
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

        //check if instructor was assigned to this course selected by guest in dropdown
        public bool CheckInstructorAssignedToCourse(string courseChosen, int instrucId)
        {
            //retrieve chosen cours by guest in dropdown
            Course courseSelected = _courseRepository.GetCourse(Int32.Parse(courseChosen));

            bool instructorAssignedToCourse = false;
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
        public bool CheckInstructorHasFreeHours(int instrucId, byte hoursLimit)
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

            instrucHasFreeHours = (assignedHoursForInstructor <= hoursLimit) ? true : false;

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


        //REGISTER GUESTS
        //retrieve a list with all guests and their courses from repository
        public ICollection<RegisterGuestViewModel> GetRegisterGuests()
        {
            ICollection<Guest> guestsAsList = _guestRepository.GetGuests().ToList();
            List<RegisterGuestViewModel> registerGuestsViewModel = new List<RegisterGuestViewModel>();

            foreach (Guest guest in guestsAsList)
            {
                bool guestHasCourses = _courseGuestRepository.GuestHasCourse(guest.Id);
                List<string> courseNamesAsListOfStrings = new List<string> { };
                if (guestHasCourses)
                {
                    ICollection<CourseGuest> coursesForGuest = _courseGuestRepository.GetCoursesByGuestId(guest.Id);
                    foreach (CourseGuest courseForGuest in coursesForGuest)
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

            return registerGuestsViewModel;
        }

        //retrieve available courses to register for RegisterGuest by id as <SelectListItem>
        public List<SelectListItem> GetAvailableCoursesRegisterGuest(int Id)
        {
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
            return availableCoursesToAssign;
        }

        //get a list of all instructors as <SelectListItem>
        public List<SelectListItem> GetAllInstructorsRegisterGuest()
        {
            List<Instructor> allInstructorsFromDb = _instructorRepository.GetInstructors().ToList();
            List<SelectListItem> allInstructors = new List<SelectListItem>();
            foreach (Instructor instruc in allInstructorsFromDb)
            {
                allInstructors.Add(new SelectListItem { Text = instruc.FirstName + ' ' + instruc.LastName, Value = instruc.Id.ToString() });
            }
            return allInstructors;
        }

        //check if instructor selected in dropdown was assigned to this course selected in dropdown
        public bool CheckInstructorAssignedToCourse(string courseChosen, string instrucChosen)
        {
            //retrieve chosen instructor by guest in dropdown 
            Instructor instructorSelected = _instructorRepository.GetInstructor(Int32.Parse(instrucChosen));

            bool instructorAssignedToCourse = CheckInstructorAssignedToCourse(courseChosen, instructorSelected.Id);
            return instructorAssignedToCourse;
        }

        //check if guest was already registered to this course selected in dropdown
        public bool CheckGuestAssignedToCourse(string courseChosen, int guestId)
        {
            //retrieve chosen course by guest in dropdown 
            Course courseSelected = _courseRepository.GetCourse(Int32.Parse(courseChosen));

            bool guestAssignedToCourse = false;
            List<CourseGuest> coursesOfGuest = _courseGuestRepository.GetCoursesByGuestId(guestId).ToList(); ;
            foreach (CourseGuest course in coursesOfGuest)
            {
                if (course.CourseId == courseSelected.Id)
                {
                    guestAssignedToCourse = true;
                    break;
                }
            }
            return guestAssignedToCourse;
        }

        //check if guest was already registered to this instructor selected in dropdown
        public bool CheckGuestAssignedToInstructor(string instrucChosen, int guestId)
        {
            //retrieve chosen instructor by guest in dropdown 
            Instructor instructorSelected = _instructorRepository.GetInstructor(Int32.Parse(instrucChosen));

            bool guestAssignedToInstructor = false;
            List<InstructorGuest> guestsOfInstructor = _instructorGuestRepository.GetGuestsByInstructorId(instructorSelected.Id).ToList();
            foreach (InstructorGuest guest in guestsOfInstructor)
            {
                if (guest.GuestId == guestId)
                {
                    guestAssignedToInstructor = true;
                    break;
                }
            }
            return guestAssignedToInstructor;
        }

        //check if instructor has free hours equal at least with the hours demanded by the guest
        public bool CheckInstructorHasFreeHours(string instrucChosen, byte hoursLimit)
        {
            //retrieve chosen instructor by guest in dropdown 
            Instructor instructorSelected = _instructorRepository.GetInstructor(Int32.Parse(instrucChosen));

            bool instrucHasCourses = CheckInstructorHasFreeHours(instructorSelected.Id, hoursLimit);
            return instrucHasCourses;
        }

        //registering a guest to a course
        public bool RegisterGuest(CourseGuest courseGuest)
        {
            return _courseGuestRepository.RegisterGuest(courseGuest);
        }

        //registering a guest to an instructor
        public bool RegisterGuest(InstructorGuest instructorGuest)
        {
            return _instructorGuestRepository.RegisterGuest(instructorGuest);
        }


    }
}
