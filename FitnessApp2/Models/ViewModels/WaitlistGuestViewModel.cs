using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp2.Models.ViewModels
{
    public class WaitlistGuestViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Range(1, 5)]
        public byte Hours { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }
    }
}
