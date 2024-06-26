﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp2.Models.DbEntities
{
    public class Guest
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        public DateTime? AddedDate { get; set; }

        [Range(1,5)]
        public byte Hours { get; set; }

        public int? DetailId { get; set; }
        public Detail? Detail { get; set; }

        public int? SectionId { get; set; }
        public Section? Section { get; set; }

        public ICollection<CourseGuest>? CourseGuests { get; set; }
        public ICollection<InstructorGuest>? InstructorGuests { get; set; }
    }
}
