using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp2.Models.DbEntities
{
    public class Instructor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Required]
        public int InstructorID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        //[Required]
        public required string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        //[Required]
        public required string LastName { get; set; }

        public DateTime? AddedDate { get; set; }

        [Range(1, 10)]
        public byte? ExperienceYears { get; set; }

        [RegularExpression(@"^[MF]$")]
        public char? Gender { get; set; }

        //[Required]
        public required ICollection<Client> Clients { get; set; }
    }
}
