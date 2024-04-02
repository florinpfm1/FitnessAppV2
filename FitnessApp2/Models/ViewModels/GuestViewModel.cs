using System.ComponentModel;

namespace FitnessApp2.Models.ViewModels
{
    public class GuestViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Date Added")]
        public DateTime? AddedDate { get; set; }

        [DisplayName("Hours/Week")]
        public byte Hours { get; set; }

        public string SectionName { get; set; }

        //public string? SectionName { get; set; }
        
        //public string InstructorName { get; set; }

        //public string CourseName { get; set; }
        

        /*
        [DisplayName("Name")]
        public string FullName { get { return FirstName + " " + LastName; } }
        */
    }
}
