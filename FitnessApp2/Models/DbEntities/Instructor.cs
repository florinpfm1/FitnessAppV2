using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp2.Models.DbEntities
{
    public class Instructor
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        public DateTime? AddedDate { get; set; }

        [Range(1, 10)]
        public byte? ExperienceYears { get; set; }

        [RegularExpression(@"^[MF]$")]
        public char? Gender { get; set; }

        public ICollection<CourseInstructor>? CourseInstructors { get; set; }
        public ICollection<InstructorGuest>? InstructorGuests { get; set; }
    }
}
