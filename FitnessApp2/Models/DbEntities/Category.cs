using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp2.Models.DbEntities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Required]
        public int CategoryID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        //[Required]
        public required string Name { get; set; }

        //[Required]
        public required ICollection<Client> Clients { get; set; }
    }
}
