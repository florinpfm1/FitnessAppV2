﻿using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp2.Controllers
{
    public class AssignInstructorController : Controller
    {
        private readonly IFitnessServices _fitnessServices;
        public AssignInstructorController(IFitnessServices fitnessServices)
        {
            this._fitnessServices = fitnessServices;
        }

        //--------------- RETRIEVE ALL INSTRUCTORS AND THEIR COURSES ---------------
        [HttpGet]
        public IActionResult GetAssignInstructors()
        {
            ICollection<AssignInstructorViewModel> assignInstructorsViewModel = _fitnessServices.GetAssignInstructors();
            return View(assignInstructorsViewModel);
        }

        //--------------- ASSIGN INSTRUCTOR TO COURSE ---------------
        [HttpGet]
        public IActionResult EditAssignInstructor(int Id)
        {
            try
            {
                //check if instructor with id exists in db
                bool instructorExists = _fitnessServices.InstructorExists(Id);
                if (!instructorExists)
                {
                    TempData["errorMessage"] = $"Instructor with the InstructorID: {Id} is not found.";
                    return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                }

                //retrieve instructor info from db and prepare AssignInstructorViewModel for POST below
                AssignInstructorViewModel assignInstructorViewModel = _fitnessServices.BuildAssignInstructorViewModel(Id);
                return View(assignInstructorViewModel);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetAssignInstructors", "AssignInstructor");
            }
        }

        [HttpPost]
        public IActionResult EditAssignInstructor(AssignInstructorViewModel assignInstructorViewModel)
        {
            try
            {
                //check model
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View(assignInstructorViewModel);
                }

                //check if instructor was assigned to this course selected by guest in dropdown
                bool instructorAssignedToCourse = _fitnessServices.CheckInstructorAssignedToCourse(assignInstructorViewModel.CourseSelected, assignInstructorViewModel.Id);
                if (instructorAssignedToCourse)
                {
                    TempData["errorMessage"] = "Instructor is already assigned to this course.";
                    AssignInstructorViewModel rebuiltAssignInstructorViewModel = _fitnessServices.BuildAssignInstructorViewModel(assignInstructorViewModel.Id);
                    return View("EditAssignInstructor", rebuiltAssignInstructorViewModel);
                }

                //check if instructor has at least 5 hours free to take on one more course with at least 1...5 guests (each guest can have between 1...5 hours)
                bool instrucHasFreeHours = _fitnessServices.CheckInstructorHasFreeHours(assignInstructorViewModel.Id, (byte)35, "forAssignInstructor", (byte)0);
                if (!instrucHasFreeHours)
                {
                    TempData["errorMessage"] = "Instructor does not have free hours to start a new course.";
                    AssignInstructorViewModel rebuiltAssignInstructorViewModel = _fitnessServices.BuildAssignInstructorViewModel(assignInstructorViewModel.Id);
                    return View("EditAssignInstructor", rebuiltAssignInstructorViewModel);
                }

                //check if instructor is already assigned to a course
                bool instrucHasCourse = _fitnessServices.InstructorHasCourse(assignInstructorViewModel.Id);
                if (instrucHasCourse)
                {
                    TempData["errorMessage"] = "Instructor is already assigned to a course.";
                    AssignInstructorViewModel rebuiltAssignInstructorViewModel = _fitnessServices.BuildAssignInstructorViewModel(assignInstructorViewModel.Id);
                    return View("EditAssignInstructor", rebuiltAssignInstructorViewModel);
                }

                //retrieve the selected course by its name
                Course courseSelected = _fitnessServices.GetCourse(Int32.Parse(assignInstructorViewModel.CourseSelected));

                //create link in db instructor<->course, create and save CourseInstructor to db context
                CourseInstructor courseInstructor = new CourseInstructor()
                {
                    CourseId = courseSelected.Id,
                    InstructorId = assignInstructorViewModel.Id
                };

                bool statusAssignInstructorInDb = _fitnessServices.AssignInstructor(courseInstructor);
                if (statusAssignInstructorInDb)
                {
                    TempData["successMessage"] = "Instructor assigned successfully!";
                    return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong when saving to database.";
                    return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetAssignInstructors", "AssignInstructor");
            }
        }

        //--------------- DE-ASSIGN INSTRUCTOR FROM ALL HIS COURSES ---------------
        [HttpGet]
        public IActionResult DeleteAssignInstructor(int Id)
        {
            //build InstructorViewModel
            Instructor instructor = _fitnessServices.GetInstructor(Id);
            InstructorViewModel instructorViewModel = new InstructorViewModel();
            instructorViewModel.Id = instructor.Id;
            instructorViewModel.FirstName = instructor.FirstName;
            instructorViewModel.LastName = instructor.LastName;
            return View(instructorViewModel);
        }

        [HttpPost]
        public IActionResult DeleteAssignInstructor(InstructorViewModel instructorViewModel)
        {
            try
            {
                //check if instructor with id exists in db
                bool instructorExists = _fitnessServices.InstructorExists(instructorViewModel.Id);
                if (!instructorExists)
                {
                    TempData["errorMessage"] = $"Instructor with the InstructorID: {instructorViewModel.Id} is not found.";
                    return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                }

                //check if instructor is assigned to at least one course
                bool instrucIsAssignedToAnyCourse = _fitnessServices.InstructorHasCourse(instructorViewModel.Id);
                if (!instrucIsAssignedToAnyCourse)
                {
                    TempData["errorMessage"] = $"Instructor with the InstructorID: {instructorViewModel.Id} is not assigned to any course.";
                    return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                }

                //build ScheduleInstructorViewModel - with all his courses and guests of each course
                ScheduleInstructorViewModel myScheduleInstructorViewModel = new ScheduleInstructorViewModel();
                myScheduleInstructorViewModel = _fitnessServices.BuildScheduleForInstructor(instructorViewModel.Id);
                var myListCrsAndGst = myScheduleInstructorViewModel.CrsAndGst; //courses and their guests to register again

                //initialize list of instructor assigned courses
                List<CourseInstructor> listCourseInstructor = new List<CourseInstructor>();

                //initialize status for deregistration of guest from a course and instructor
                bool statusDeleteGuest = true;
                bool statusReregisterGuest = true;

                foreach (var crsAndGst in myListCrsAndGst)
                {
                    if (crsAndGst.Crs is not null)
                    {
                        if (crsAndGst.Gst.Count >= 1)
                        {
                            foreach (Guest currentGuest in crsAndGst.Gst)
                            {
                                //delete current guest from course and from instructor
                                CourseGuest courseGuestToDeregister = _fitnessServices.GetCourseGuestByCourseIdAndGuestId(crsAndGst.Crs.Id, currentGuest.Id);
                                InstructorGuest instructorGuestToDeregister = _fitnessServices.GetInstructorGuestByInstructorIdAndGuestId(instructorViewModel.Id, currentGuest.Id);
                                bool statusDeleteGuestToCourseInDb = _fitnessServices.DeleteCourseGuest(courseGuestToDeregister);
                                bool statusDeleteGuestToInstructorInDb = _fitnessServices.DeleteInstructorGuest(instructorGuestToDeregister);

                                if (!(statusDeleteGuestToCourseInDb && statusDeleteGuestToInstructorInDb))
                                {
                                    statusDeleteGuest = false;
                                }

                                //build List instructors with free hours
                                List<ScheduleInstructorViewModel> scheduleInstructorsViewModel = new List<ScheduleInstructorViewModel>();
                                scheduleInstructorsViewModel = _fitnessServices.BuildScheduleForInstructors().ToList();
                                List<ScheduleInstructorViewModel> scheduleInstructorViewModelsWithoutCurrent = new List<ScheduleInstructorViewModel>();
                                ScheduleInstructorViewModel currentScheduleInstructor = _fitnessServices.BuildScheduleForInstructor(instructorViewModel.Id);
                                
                                foreach (ScheduleInstructorViewModel elem in scheduleInstructorsViewModel)
                                {
                                    if (elem.Id != instructorViewModel.Id) 
                                    {
                                        scheduleInstructorViewModelsWithoutCurrent.Add(elem);
                                    }
                                }
                                List<ScheduleInstructorViewModel> freeHoursScheduleInstructorsViewModel = scheduleInstructorViewModelsWithoutCurrent.Where(elem => elem.FreeHours >= 5 && elem.FreeHours <= 40).OrderByDescending(elem => elem.FreeHours).ToList();

                                //filter List instructors only the ones with assigned courses; also order descending by free hours
                                List<ScheduleInstructorViewModel> withAssignedCoursesScheduleInstructorsViewModel = freeHoursScheduleInstructorsViewModel.Where(elem => elem.CrsAndGst != null).OrderByDescending(elem => elem.FreeHours).ToList(); ;

                                if (withAssignedCoursesScheduleInstructorsViewModel.Count >= 1)
                                {
                                    //register currentGuest to myListInstructors[0].Id - is instructor with the most amount of free hours
                                    InstructorGuest instructorGuestToRegister = new InstructorGuest()
                                    {
                                        InstructorId = withAssignedCoursesScheduleInstructorsViewModel[0].Id,
                                        GuestId = currentGuest.Id
                                    };

                                    //obtain list of courses from myListInstructors[0].Id
                                    List<CourseInstructor> coursesForFirstInstructor = _fitnessServices.GetCoursesByInstructorId(withAssignedCoursesScheduleInstructorsViewModel[0].Id).ToList();

                                    //register currentGuest to first course
                                    CourseGuest courseGuestToRegister = new CourseGuest()
                                    {
                                        CourseId = coursesForFirstInstructor[0].CourseId,
                                        GuestId = currentGuest.Id
                                    };

                                    bool statusRegisterGuestToCourseInDb = _fitnessServices.RegisterGuest(courseGuestToRegister);
                                    bool statusRegisterGuestToInstructorInDb = _fitnessServices.RegisterGuest(instructorGuestToRegister);

                                    if (!(statusRegisterGuestToCourseInDb && statusRegisterGuestToInstructorInDb))
                                    {
                                        statusReregisterGuest = false;
                                    }
                                }
                            }
                        }
                        
                        //when Course exists add CourseInstructor to list of instructor assigned courses
                        CourseInstructor currentAssignedCourse = _fitnessServices.GetCourseInstructorByCourseIdAndInstructorId(crsAndGst.Crs.Id, instructorViewModel.Id);
                        listCourseInstructor.Add(currentAssignedCourse);
                    }
                }
                //delete instructor from all his courses
                bool statusDeleteCourseToInstructorInDb = _fitnessServices.DeleteAllCourseInstructor(listCourseInstructor);

                if (statusDeleteCourseToInstructorInDb && statusDeleteGuest && statusReregisterGuest)
                {
                    TempData["successMessage"] = "Instructor deassigned successfully!";
                    return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                }
                else
                {
                    TempData["errorMessage"] = $"Something went wrong when deleting from database. " +
                        $"Instructor deassigned from course has status {statusDeleteCourseToInstructorInDb} " +
                        $"Guests deregistered from courses has status {statusDeleteGuest}" +
                        $"Guests reregistered from courses has status {statusReregisterGuest}";
                    return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetAssignInstructors", "AssignInstructor");
            }
        }
    }
}
