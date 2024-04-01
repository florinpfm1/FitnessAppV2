using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FitnessApp2.Data;
using System;
using System.Linq;
using FitnessApp2.Models.DbEntities;
using System.Reflection;

namespace FitnessApp2
{
    public class Seed
    {
        private readonly FAppDbContext dataContext;
        public Seed(FAppDbContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.CourseInstructors.Any())
            {
                var courseInstructors = new List<CourseInstructor>()
                {
                    new CourseInstructor()
                    {
                        Course = new Course()
                        {
                            Name = "Speed Cycling Hardcore",
                            CourseGuests = new List<CourseGuest>()
                            {
                                new CourseGuest()
                                {
                                    Guest = new Guest()
                                    {
                                        FirstName = "Minnie",
                                        LastName = "Ellie",
                                        AddedDate = DateTime.Now,
                                        Hours = 2,
                                        Detail = new Detail() { Email = "minnie@gmail.com", Phone = "0745888111" },
                                        Section = new Section() { Name = "School" }
                                    }
                                }
                            }
                        },
                        Instructor = new Instructor()
                            {
                                FirstName = "John",
                                LastName = "Green",
                                AddedDate = DateTime.Now,
                                ExperienceYears = 1,
                                Gender = 'M'
                            }
                    },
                    new CourseInstructor()
                    {
                        Course = new Course()
                        {
                            Name = "Fat Burning Extreme",
                            CourseGuests = new List<CourseGuest>()
                            {
                                new CourseGuest()
                                {
                                    Guest = new Guest()
                                    {
                                        FirstName = "Tom",
                                        LastName = "Rooney",
                                        AddedDate = DateTime.Now,
                                        Hours = 3,
                                        Detail = new Detail() { Email = "tom@gmail.com", Phone = "0733111222" },
                                        Section = new Section() { Name = "Student" }
                                    }
                                }
                            }
                        },
                        Instructor = new Instructor()
                            {
                                FirstName = "Sonya",
                                LastName = "Steel",
                                AddedDate = DateTime.Now,
                                ExperienceYears = 8,
                                Gender = 'F'
                            }
                    },
                    new CourseInstructor()
                    {
                        Course = new Course()
                        {
                            Name = "Boxing For Beginners",
                            CourseGuests = new List<CourseGuest>()
                            {
                                new CourseGuest()
                                {
                                    Guest = new Guest()
                                    {
                                        FirstName = "Steve",
                                        LastName = "Star",
                                        AddedDate = DateTime.Now,
                                        Hours = 4,
                                        Detail = new Detail() { Email = "steve@gmail.com", Phone = "0700444555" },
                                        Section = new Section() { Name = "Employee" }
                                    }
                                }
                            }
                        },
                        Instructor = new Instructor()
                            {
                                FirstName = "Karl",
                                LastName = "Budd",
                                AddedDate = DateTime.Now,
                                ExperienceYears = 6,
                                Gender = 'M'
                            }
                    },
                    new CourseInstructor()
                    {
                        Course = new Course()
                        {
                            Name = "Running In Group",
                            CourseGuests = new List<CourseGuest>()
                            {
                                new CourseGuest()
                                {
                                    Guest = new Guest()
                                    {
                                        FirstName = "Darla",
                                        LastName = "Kim",
                                        AddedDate = DateTime.Now,
                                        Hours = 2,
                                        Detail = new Detail() { Email = "darla@gmail.com", Phone = "0711666777" },
                                        Section = new Section() { Name = "Retired" }
                                    }
                                }
                            }
                        },
                        Instructor = new Instructor()
                            {
                                FirstName = "Vivie",
                                LastName = "Samuels",
                                AddedDate = DateTime.Now,
                                ExperienceYears = 1,
                                Gender = 'F'
                            }
                    },
                    new CourseInstructor()
                    {
                        Course = new Course()
                        {
                            Name = "Streching For All",
                            CourseGuests = new List<CourseGuest>()
                            {
                                new CourseGuest()
                                {
                                    Guest = new Guest()
                                    {
                                        FirstName = "Jay",
                                        LastName = "Alvarez",
                                        AddedDate = DateTime.Now,
                                        Hours = 1,
                                        Detail = new Detail() { Email = "jay@gmail.com", Phone = "0799000777" },
                                        Section = new Section() { Name = "Free of charge" }
                                    }
                                }
                            }
                        },
                        Instructor = new Instructor()
                            {
                                FirstName = "Bruce",
                                LastName = "Rudolf",
                                AddedDate = DateTime.Now,
                                ExperienceYears = 7,
                                Gender = 'M'
                            }
                    },
                    new CourseInstructor()
                    {
                        Course = new Course()
                        {
                            Name = "Freestyle Dancing",
                            CourseGuests = new List<CourseGuest>()
                            {
                                new CourseGuest()
                                {
                                    Guest = new Guest()
                                    {
                                        FirstName = "Matthew",
                                        LastName = "Phillips",
                                        AddedDate = DateTime.Now,
                                        Hours = 1,
                                        Detail = new Detail() { Email = "matt@gmail.com", Phone = "0799000777" },
                                        Section = new Section() { Name = "State Discount 100%" }
                                    }
                                }
                            }
                        },
                        Instructor = new Instructor()
                            {
                                FirstName = "Sam",
                                LastName = "Stark",
                                AddedDate = DateTime.Now,
                                ExperienceYears = 3,
                                Gender = 'M'
                            }
                    }
                };
                dataContext.CourseInstructors.AddRange(courseInstructors);
                dataContext.SaveChanges();
            }
        }
    }
}
