using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessApp2.Services
{
    public interface IFitnessServices
    {
        //======================================   INSTRUCTORS   ======================================
        //retrieve a list with all instructors from repository
        public ICollection<Instructor> GetInstructors();

        //retrieve one instructor by Id
        public Instructor GetInstructor(int id);

        //retrieve one instructor by FirstName and LastName
        public Instructor GetInstructor(string firstName, string lastName);

        //check if an instructors exists in db with that Id
        public bool InstructorExists(int instrucId);

        //check if instructor is assigned to at least one course
        public bool InstructorHasCourse(int instrucId);

        //~~~ CREATING a new INSTRUCTOR ~~~
        public bool CreateInstructor(Instructor instructor);

        //~~~ UPDATING an existing INSTRUCTOR ~~~
        public bool UpdateInstructor(Instructor instructor);

        //~~~ DELETING an existing INSTRUCTOR ~~~
        public bool DeleteInstructor(Instructor instructor);


        //======================================   GUESTS   ======================================
        //retrieve all assigned guests from repository
        public ICollection<Guest> GetAssignedGuests();

        //retrieve all unassigned guests from repository
        public ICollection<Guest> GetUnassignedGuests();

        //get one guest by Id
        public Guest GetGuest(int id);

        //get one guest by FirstName and LastName
        public Guest GetGuest(string firstName, string lastName);

        //get one section by Id
        public Section GetSection(int? id);

        //get one section by Name
        public Section GetSection(string name);

        //get one detail by Id
        public Detail GetDetail(int? id);

        //check if guest with the specified Id exists in db
        public bool GuestExists(int guestId);

        //check if guest is registered to at least one course
        public bool GuestHasCourse(int guestId);

        //~~~ CREATING a new GUEST ~~~
        public bool CreateGuest(Guest guest);

        //~~~ UPDATING an existing GUEST ~~~
        public bool UpdateGuest(Guest guest);

        //~~~ DELETING an existing GUEST ~~~
        public bool DeleteGuest(Guest guest);


        //======================================   WAITLIST GUESTS   ======================================
        //retrieve detail by Email and Phone
        public Detail GetDetailByPhoneAndEmail(string email, string phone);

        //~~~ CREATING a new detail ~~~
        public bool CreateDetail(Detail detail);


        //======================================   ASSIGN INSTRUCTORS   ======================================
        //retrieve a list with all instructors and their courses from repository
        public ICollection<AssignInstructorViewModel> GetAssignInstructors();
        
        //build AssignInstructorViewModel with dropdown options
        public AssignInstructorViewModel BuildAssignInstructorViewModel(int Id);

        //retrieve available courses to be assigned for AssignInstructor by Id as <SelectListItem>
        public List<SelectListItem> GetAvailableCoursesAssignInstruc(int Id);

        //check if instructor was assigned to this course selected by guest in dropdown
        public bool CheckInstructorAssignedToCourse(string courseChosen, int instrucId);

        //calculate free hours for instructor
        public byte CalculateFreeHoursInstructor(int instrucId);

        //check if instructor has at least 5 hours free to take on one more course with at least 1...5 guests (each guest can have between 1...5 hours)
        //check if instructor has free hours equal at least with the hours demanded by the new registered guest
        public bool CheckInstructorHasFreeHours(int instrucId, byte hoursLimit, string forAction, byte? guestHours);

        //get one course by Id
        public Course GetCourse(int id);

        //get one course by Name
        public Course GetCourse(string name);

        //~~~ ASSIGNING an INSTRUCTOR to a course ~~~
        public bool AssignInstructor(CourseInstructor courseInstructor);


        //======================================   REGISTER GUESTS   ======================================
        //retrieve a list with all guests and their courses from repository
        public ICollection<RegisterGuestViewModel> GetRegisterGuests();

        //build RegisterGuestViewModel with dropdown options as <SelectListItem>
        public RegisterGuestViewModel BuildRegisterGuestViewModel(int Id);

        //retrieve available courses to register for RegisterGuest by Id as <SelectListItem>
        public List<SelectListItem> GetAvailableCoursesRegisterGuest(int Id);

        //get a list of all instructors as <SelectListItem>
        public List<SelectListItem> GetAllInstructorsRegisterGuest();
        
        //check if instructor selected in dropdown was assigned to this course selected in dropdown
        public bool CheckInstructorAssignedToCourse(string courseChosen, string instrucChosen);

        //check if guest was already registered to this course selected in dropdown
        public bool CheckGuestAssignedToCourse(string courseChosen, int guestId);

        //check if guest was already registered to this instructor selected in dropdown
        public bool CheckGuestAssignedToInstructor(string instrucChosen, int guestId);

        //check if guest is already registered to any instructor
        public bool InstructorHasGuests(int guestId);

        //~~~ REGISTERING a GUEST to a COURSE ~~~
        public bool RegisterGuest(CourseGuest courseGuest);

        //~~~ REGISTERING a GUEST to an INSTRUCTOR ~~~
        public bool RegisterGuest(InstructorGuest instructorGuest);


        //====================================== UN-ASSIGN / UN-REGISTER operations //======================================
        //__________ for RegisterGuest __________
        //deleting one guest from all registered courses and all registered instructors
        public ICollection<CourseGuest> GetCoursesByGuestId(int guestId);
        public ICollection<InstructorGuest> GetInstructorsByGuestId(int guestId);
        public bool DeleteAllCourseGuest(List<CourseGuest> listOfCourseGuest);
        public bool DeleteAllInstructorGuest(List<InstructorGuest> listOfInstructorGuest);

        //deleting one guest from a registered course and from a registered instructor
        public CourseGuest GetCourseGuestByCourseIdAndGuestId(int courseId, int guestId);
        public InstructorGuest GetInstructorGuestByInstructorIdAndGuestId(int instrucId, int guestId);
        public bool DeleteCourseGuest(CourseGuest courseGuest);
        public bool DeleteInstructorGuest(InstructorGuest instructorGuest);

        //__________ for AssignInstructor __________
        //deleting an instructor from all assigned courses and for guests of each course deregister them and register to instructors with free hours
        public ICollection<CourseInstructor> GetInstructorsByCourseId(int courseId);
        public ICollection<CourseInstructor> GetCoursesByInstructorId(int instrucId);
        public CourseInstructor GetCourseInstructorByCourseIdAndInstructorId(int courseId, int instrucId);
        public bool DeleteAllCourseInstructor(List<CourseInstructor> listOfCourseInstructor);
        public bool DeleteCourseInstructor(CourseInstructor courseInstructor);


        //====================================== SCHEDULE INSTRUCTORS ======================================
        //for one instructor retrieve all his courses and all guests of each course
        public ScheduleInstructorViewModel BuildScheduleForInstructor(int instrucId);

        //for all instructors retrieve all their courses and all the guests of each course
        public ICollection<ScheduleInstructorViewModel> BuildScheduleForInstructors();
    }
}
