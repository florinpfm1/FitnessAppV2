using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp2.Models.DbEntities
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Required]
        public int ClientID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        //[Required]
        public required string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        //[Required]
        public required string LastName { get; set; }

        public DateTime? AddedDate { get; set; }

        [Range(1,5)]
        //[Required]
        public required byte Hours { get; set; }

        [ForeignKey("CategoryID")]
        //[Required]
        public int CategoryID { get; set; }
        public required Category Category { get; set; }

        [ForeignKey("InstructorID")]
        //[Required]
        public int InstructorID { get; set; }
        public required Instructor Instructor { get; set; }


    }
}
