using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FitnessApp2.Repository
{
    public class InstructorRepository : IInstructorRepository
    {
        public FAppDbContext _context { get; }
        public InstructorRepository(FAppDbContext context)
        {
           this._context = context;
        }

        //get a List of all instructors
        public ICollection<Instructor> GetInstructors()
        {
            return _context.Instructors.OrderBy(i => i.Id).ToList(); 
        }

        //get one instructor by Id
        public Instructor GetInstructor(int id)
        {
            return _context.Instructors.Where(i => i.Id == id).FirstOrDefault();
        }

        //get one instructor by FirstName and LastName
        public Instructor GetInstructor(string firstName, string lastName)
        {
            return _context.Instructors.Where(i => i.FirstName == firstName && i.LastName == lastName).FirstOrDefault();
        }

        //get a List of all instructors by Date
        public ICollection<Instructor> GetInstructors(DateOnly date)
        {
            throw new NotImplementedException();
        }

        //get a List of all instructors by ExperienceYears
        public ICollection<Instructor> GetInstructors(byte expYears)
        {
            return _context.Instructors.Where(i => i.ExperienceYears == expYears).OrderBy(i => i.Id).ToList();
        }

        //get a List of all instructors by Gender
        public ICollection<Instructor> GetInstructors(char gender)
        {
            return _context.Instructors.Where(i => i.Gender == gender).OrderBy(i => i.Id).ToList();
        }

        //check if an instructors exists in db with that Id
        public bool InstructorExists(int instrucId)
        {
            return _context.Instructors.Any(i => i.Id == instrucId);
        }

        //creating a new instructor
        public bool CreateInstructor(Instructor instructor)
        {
            _context.Add(instructor);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); 
            return saved > 0 ? true : false;
        }

        //updating an existing instructor
        public bool UpdateInstructor(Instructor instructor)
        {
            _context.Update(instructor);
            return Save();
        }

        //deleting an existing instructor
        public bool DeleteInstructor(Instructor instructor)
        {
            _context.Remove(instructor);
            return Save();
        }
    }
}
