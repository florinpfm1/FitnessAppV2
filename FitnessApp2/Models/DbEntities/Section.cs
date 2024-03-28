using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp2.Models.DbEntities
{
    public class Section
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        public ICollection<Guest>? Guests { get; set; }
    }
}
