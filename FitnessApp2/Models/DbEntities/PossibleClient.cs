using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp2.Models.DbEntities
{
    public class PossibleClient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Required]
        public int PossibleClientID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        //[Required]
        public required string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        //[Required]
        public required string LastName { get; set; }

        [EmailAddress]
        //[Required]
        public required string Email { get; set; }

        [Phone]
        public string? Phone { get; set; }
    }
}
