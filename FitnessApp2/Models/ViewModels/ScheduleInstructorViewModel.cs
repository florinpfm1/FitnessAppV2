using FitnessApp2.Models.DbEntities;
using System.ComponentModel;

namespace FitnessApp2.Models.ViewModels
{
    public class ScheduleInstructorViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Free Hours")]
        public byte FreeHours { get; set; }

        [DisplayName("Reserved Hours")]
        public byte ReservedHours { get; set; }

        [DisplayName("Max Hours")]
        public byte MaxHours { get; set; }

        public List<dynamic>? CrsAndGst {  get; set; }
    }
}
