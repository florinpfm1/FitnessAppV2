﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace FitnessApp2.Models.ViewModels
{
    public class AssignInstructorViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public List<string>? AssignedCourses { get; set; }

        public string? CourseSelected { get; set; }

        public List<SelectListItem>? AvailableCoursesToAssign { get; set; }
    }
}
