using System.ComponentModel;

namespace FitnessApp2.Models.ViewModels
{
    public class AssignInstructorViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string CourseName { get; set; }
    }
}
