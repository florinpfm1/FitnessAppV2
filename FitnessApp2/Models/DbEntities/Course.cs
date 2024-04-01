using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp2.Models.DbEntities
{
    public class Course
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Difficulty {  get; set; }
        
        public byte? Rating { get; set; }

        public ICollection<CourseInstructor>? CourseInstructors { get; set; }

        public ICollection<CourseGuest>? CourseGuests { get; set;}
    }
}
