using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FitnessApp2.Services
{
    public interface IFitnessServices
    {
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~ READ METHODS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 

        //INSTRUCTORS
        //retrieve a list with all instructors from repository
        public ICollection<Instructor> GetInstructors();
        //retrieve one instructor by Id
        public Instructor GetInstructor(int id);
        //retrieve one instructor by FirstName and LastName
        public Instructor GetInstructor(string firstName, string lastName);

        //checks if an instructors exists in db with that Id
        public bool InstructorExists(int instrucId);


        //creating a new instructor
        public bool CreateInstructor(Instructor instructor);

        //updating an existing instructor
        public bool UpdateInstructor(Instructor instructor);

        //deleting an existing instructor
        public bool DeleteInstructor(Instructor instructor);


        //GUESTS
        //retrieve all assigned guests from repository
        public ICollection<Guest> GetAssignedGuests();

        //retrieve all unassigned guests from repository
        public ICollection<Guest> GetUnassignedGuests();

        //get one guest by id
        public Guest GetGuest(int id);

        //get one guest by FirstName and LastName
        public Guest GetGuest(string firstName, string lastName);

        //get one section by Id
        public Section GetSection(int? id);

        //get one section by Name
        public Section GetSection(string name);

        //get one detail by id
        public Detail GetDetail(int? id);

        //checks if guest with the specified Id exists in db
        public bool GuestExists(int guestId);


        //creating a new guest
        public bool CreateGuest(Guest guest);

        //updating an existing instructor
        public bool UpdateGuest(Guest guest);

        //deleteing an existing instructor
        public bool DeleteGuest(Guest guest);


        //ASSIGN INSTRUCTORS
        //retrieve a list with all instructors and their courses from repository
        public ICollection<AssignInstructorViewModel> GetAssignInstructors();
        //retrieve assigned courses for AssignInstructor by id
        public List<SelectListItem> GetAssignedCoursesAssignInstruc(int Id);
        //check if instructor was assigned to this course
        public bool CheckInstructorAssignedToCourse(string courseChosen, int instrucId);
        //check if instructor has at least 5 hours free to take on one more guest (which can choose between 1...5 hours)
        public bool CheckInstructorHasFreeHours(int instrucId);
        //get one course by Id
        public Course GetCourse(int id);
        //get one course by Name
        public Course GetCourse(string name);


        //assigning an instructor to a course
        public bool AssignInstructor(CourseInstructor courseInstructor);
    }
}
