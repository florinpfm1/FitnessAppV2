using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp2.Services
{
    public class FitnessServices : IFitnessServices
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IDetailRepository _detailRepository;

        public FitnessServices(
            IInstructorRepository instructorRepository,
            IGuestRepository guestRepository,
            ISectionRepository sectionRepository,
            IDetailRepository detailRepository
            )
        {
            this._instructorRepository = instructorRepository;
            this._guestRepository = guestRepository;
            this._sectionRepository = sectionRepository;
            this._detailRepository = detailRepository;
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

    }
}
