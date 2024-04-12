using System.ComponentModel;

namespace FitnessApp2.Models.ViewModels
{
    public class InstructorViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Date Added")]
        public DateTime? AddedDate { get; set; }

        [DisplayName("Years Experience")]
        public byte? ExperienceYears { get; set; }

        public char? Gender { get; set; }
    }
}
