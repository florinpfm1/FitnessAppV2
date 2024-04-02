using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace FitnessApp2.Controllers
{
    public class ScheduleInstructorController : Controller
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly ICourseInstructorRepository _courseInstructorRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseGuestRepository _courseGuestRepository;
        private readonly IInstructorGuestRepository _instructorGuestRepository;

        public ScheduleInstructorController(
            IInstructorRepository instructorRepository, 
            IGuestRepository guestRepository,
            ICourseInstructorRepository courseInstructorRepository,
            ICourseRepository courseRepository,
            ICourseGuestRepository courseGuestRepository,
            IInstructorGuestRepository instructorGuestRepository
            )
        {
            this._instructorRepository = instructorRepository;
            this._guestRepository = guestRepository;
            this._courseInstructorRepository = courseInstructorRepository;
            this._courseRepository = courseRepository;
            this._courseGuestRepository = courseGuestRepository;
            this._instructorGuestRepository = instructorGuestRepository;
        }

        [HttpGet]
        public IActionResult GetScheduleInstructors()
        {
            ICollection<Instructor> instructors = _instructorRepository.GetInstructors();
            List<Instructor> instructorsAsList = instructors.ToList();
            List<ScheduleInstructorViewModel> scheduleInstructorsViewModel = new List<ScheduleInstructorViewModel>();

            foreach (Instructor instructor in instructorsAsList)
            {
                bool instrucHasCourses = _courseInstructorRepository.InstructorHasCourse(instructor.Id);
                byte assignedHoursForInstructor = (byte)0; //initialize instructor assigned hours from all guests

                List<dynamic> crsAndGst = new List<dynamic>();

                if (instrucHasCourses)
                {
                    ICollection<CourseInstructor> coursesForInstructor = _courseInstructorRepository.GetCoursesByInstructorId(instructor.Id);
                    foreach (CourseInstructor courseForInstructor in coursesForInstructor)
                    {
                        Course currentCourse = new Course();
                        currentCourse = _courseRepository.GetCourse(courseForInstructor.CourseId);

                        bool courseHasGuests = _courseGuestRepository.CourseHasGuests(courseForInstructor.CourseId);
                        List<Guest> guestsOfACourse = new List<Guest>();

                        if (courseHasGuests)
                        {
                            ICollection<CourseGuest> guestsForCourse = _courseGuestRepository.GetGuestsByCourseId(courseForInstructor.CourseId);
                            ICollection<InstructorGuest> guestsForInstructor = _instructorGuestRepository.GetGuestsByInstructorId(instructor.Id);
                            foreach (CourseGuest guestForCourse in guestsForCourse)
                            {
                                foreach (InstructorGuest guestForInstructor in guestsForInstructor)
                                {
                                    if (guestForInstructor.GuestId == guestForCourse.GuestId)
                                    {
                                        Guest currentGuest = new Guest();
                                        currentGuest = _guestRepository.GetGuest(guestForCourse.GuestId);
                                        guestsOfACourse.Add(currentGuest);
                                        assignedHoursForInstructor += currentGuest.Hours; //add hours of each guestOfInstructor to his instructor
                                    }
                                }
                                
                            }
                        }

                        var o1 = new { Crs = currentCourse, Gst = guestsOfACourse };
                        crsAndGst.Add(o1);
                    }
                }

                ScheduleInstructorViewModel scheduleInstructorViewModel = new ScheduleInstructorViewModel()
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    FreeHours = (byte)(40 - assignedHoursForInstructor),
                    ReservedHours = assignedHoursForInstructor,
                    MaxHours = (byte)40,
                    CrsAndGst = (instrucHasCourses) ? crsAndGst : null
                };
                scheduleInstructorsViewModel.Add(scheduleInstructorViewModel);
            }

            return View(scheduleInstructorsViewModel);
        }
    }
}
