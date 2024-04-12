using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface IInstructorRepository
    {
        //retrieving
        public ICollection<Instructor> GetInstructors();
        public Instructor GetInstructor(int id);
        public Instructor GetInstructor(string firstName, string lastName);
        public ICollection<Instructor> GetInstructors(byte expYears);
        public ICollection<Instructor> GetInstructors(char gender);
        //checking
        public bool InstructorExists(int instrucId);

        //creating
        public bool CreateInstructor(Instructor instructor);

        //updating
        public bool UpdateInstructor(Instructor instructor);

        //deleting
        public bool DeleteInstructor(Instructor instructor);
    }
}
