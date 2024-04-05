using FitnessApp2.Models.DbEntities;

namespace FitnessApp2.Interfaces
{
    public interface IInstructorRepository
    {
        //retrieving
        ICollection<Instructor> GetInstructors();
        Instructor GetInstructor(int id);
        Instructor GetInstructor(string firstName, string lastName);
        //ICollection<Instructor> GetInstructors(DateOnly date);
        ICollection<Instructor> GetInstructors(byte expYears);
        ICollection<Instructor> GetInstructors(char gender);
        bool InstructorExists(int instrucId);

        //creating
        bool CreateInstructor(Instructor instructor);

        //updating
        bool UpdateInstructor(Instructor instructor);

        //deleting
        bool DeleteInstructor(Instructor instructor);
    }
}
