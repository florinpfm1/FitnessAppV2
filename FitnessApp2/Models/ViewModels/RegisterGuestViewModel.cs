using FitnessApp2.Models.DbEntities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp2.Models.ViewModels
{
    public class RegisterGuestViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public byte? Hours { get; set; }

        [DisplayName("Assigned Courses")]
        public List<string>? AssignedCourses { get; set; }

        public string? CourseSelected { get; set; }

        public string? InstructorSelected { get; set; }

        public List<SelectListItem>? AvailableCoursesToAssign { get; set; }

        public List<SelectListItem>? AllInstructors { get; set; }
    }
}
