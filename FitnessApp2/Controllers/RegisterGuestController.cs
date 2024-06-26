﻿using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp2.Controllers
{
    public class RegisterGuestController : Controller
    {
        private readonly IFitnessServices _fitnessServices;
        public RegisterGuestController(IFitnessServices fitnessServices)
        {
            this._fitnessServices = fitnessServices;
        }

        //--------------- RETRIEVE ALL GUESTS AND THEIR COURSES ---------------
        [HttpGet]
        public IActionResult GetRegisterGuests()
        {
            ICollection<RegisterGuestViewModel> registerGuestsViewModel = _fitnessServices.GetRegisterGuests();
            return View(registerGuestsViewModel);
        }

        //--------------- REGISTER GUEST TO COURSE ---------------
        [HttpGet]
        public IActionResult EditRegisterGuest(int Id)
        {
            try
            {
                //check if guest with id exists in db
                bool guestExists = _fitnessServices.GuestExists(Id);
                if (!guestExists)
                {
                    TempData["errorMessage"] = $"Guest with the GuestID: {Id} is not found.";
                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                }

                //retrieve guest info from db and prepare RegisterGuestViewModel for POST below
                RegisterGuestViewModel registerGuestViewModel = _fitnessServices.BuildRegisterGuestViewModel(Id);
                return View(registerGuestViewModel);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetRegisterGuests", "RegisterGuest");
            }
        }

        [HttpPost]
        public IActionResult EditRegisterGuest(RegisterGuestViewModel registerGuestViewModel)
        {
            try
            {
                //check model
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View("EditRegisterGuest", registerGuestViewModel);
                }

                //check if instructor selected in dropdown was assigned to this course selected in dropdown
                bool instructorAssignedToCourse = _fitnessServices.CheckInstructorAssignedToCourse(registerGuestViewModel.CourseSelected, registerGuestViewModel.InstructorSelected); ;
                if (!instructorAssignedToCourse)
                {
                    TempData["errorMessage"] = "Chosen Instructor is not assigned to this course.";
                    RegisterGuestViewModel rebuiltRegisterGuestViewModel = _fitnessServices.BuildRegisterGuestViewModel(registerGuestViewModel.Id);
                    return View("EditRegisterGuest", rebuiltRegisterGuestViewModel);
                }

                //check if guest was already registered to this course selected in dropdown
                //check if guest was already registered to this instructor selected in dropdown
                bool guestAssignedToCourse = _fitnessServices.CheckGuestAssignedToCourse(registerGuestViewModel.CourseSelected, registerGuestViewModel.Id);
                bool guestAssignedToInstructor = _fitnessServices.CheckGuestAssignedToInstructor(registerGuestViewModel.InstructorSelected, registerGuestViewModel.Id);
                if (guestAssignedToCourse && guestAssignedToInstructor)
                {
                    TempData["errorMessage"] = "Guest is already assigned to this course.";
                    RegisterGuestViewModel rebuiltRegisterGuestViewModel = _fitnessServices.BuildRegisterGuestViewModel(registerGuestViewModel.Id);
                    return View("EditRegisterGuest", rebuiltRegisterGuestViewModel);
                }

                //retrieve the selected course and selected instructor by guest in dropdown (by its name parsed to id)
                Course courseSelected = _fitnessServices.GetCourse(Int32.Parse(registerGuestViewModel.CourseSelected));
                Instructor instructorSelected = _fitnessServices.GetInstructor(Int32.Parse(registerGuestViewModel.InstructorSelected));

                //check if instructor has free hours equal at least with the hours demanded by the new registered guest
                bool instrucHasFreeHours = _fitnessServices.CheckInstructorHasFreeHours(instructorSelected.Id, (byte)40, "forRegisterGuest", registerGuestViewModel.Hours);
                if (!instrucHasFreeHours)
                {
                    TempData["errorMessage"] = "Instructor does not have enough free hours to take the guest.";
                    RegisterGuestViewModel rebuiltRegisterGuestViewModel = _fitnessServices.BuildRegisterGuestViewModel(registerGuestViewModel.Id);
                    return View("EditRegisterGuest", rebuiltRegisterGuestViewModel);
                }

                //check if guest is already registered to a course/instructor
                bool guestHasCourses = _fitnessServices.GuestHasCourse(registerGuestViewModel.Id);
                bool instructorHasGuests = _fitnessServices.InstructorHasGuests(registerGuestViewModel.Id);
                if (guestHasCourses && instructorHasGuests)
                {
                    TempData["errorMessage"] = "Guest is already registered to a course/instructor.";
                    RegisterGuestViewModel rebuiltRegisterGuestViewModel = _fitnessServices.BuildRegisterGuestViewModel(registerGuestViewModel.Id);
                    return View("EditRegisterGuest", rebuiltRegisterGuestViewModel);
                }

                //create link in db guest<->course and guest<->instructor, create and save CourseGuest and InstructorGuest to db context
                CourseGuest courseGuest = new CourseGuest()
                {
                    CourseId = courseSelected.Id,
                    GuestId = registerGuestViewModel.Id
                };

                InstructorGuest instructorGuest = new InstructorGuest()
                {
                    InstructorId = instructorSelected.Id,
                    GuestId = registerGuestViewModel.Id
                };

                bool statusRegisterGuestToCourseInDb = _fitnessServices.RegisterGuest(courseGuest);
                bool statusRegisterGuestToInstructorInDb = _fitnessServices.RegisterGuest(instructorGuest);
                if (statusRegisterGuestToCourseInDb && statusRegisterGuestToInstructorInDb)
                {
                    TempData["successMessage"] = "Guest registered successfully!";
                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                }
                else
                {
                    TempData["errorMessage"] = $"Something went wrong when saving to database. Guest registered to course has status {statusRegisterGuestToCourseInDb} and guest registered to instructor has status {statusRegisterGuestToInstructorInDb}";
                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                } 
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetRegisterGuests", "RegisterGuest");
            }
        }

        //--------------- DE-REGISTER GUEST FROM ALL HIS COURSES ---------------
        [HttpGet]
        public IActionResult DeleteRegisterGuest(int Id)
        {
            //build GuestViewModel
            Guest guest = _fitnessServices.GetGuest(Id);
            GuestViewModel guestViewModel = new GuestViewModel();
            guestViewModel.Id = guest.Id;
            guestViewModel.FirstName = guest.FirstName;
            guestViewModel.LastName = guest.LastName;
            return View(guestViewModel);
        }

        [HttpPost]
        public IActionResult DeleteRegisterGuest(GuestViewModel guestViewModel)
        {
            try
            {
                //check if guest with id exists in db
                bool guestExists = _fitnessServices.GuestExists(guestViewModel.Id);
                if (!guestExists)
                {
                    TempData["errorMessage"] = $"Guest with the GuestID: {guestViewModel.Id} is not found.";
                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                }

                //check if guest is registered to at least one course
                bool guestIsRegisteredToAnyCourse = _fitnessServices.GuestHasCourse(guestViewModel.Id);
                if (!guestIsRegisteredToAnyCourse)
                {
                    TempData["errorMessage"] = $"Guest with the GuestID: {guestViewModel.Id} is not registered to any course.";
                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                }

                //retrieve a list of course<->guest and instructor<->guest
                List<CourseGuest> listOfCourseGuest = _fitnessServices.GetCoursesByGuestId(guestViewModel.Id).ToList();
                List<InstructorGuest> listOfInstructorGuest = _fitnessServices.GetInstructorsByGuestId(guestViewModel.Id).ToList();

                //delete guest from all his courses in db
                bool statusDeleteGuestToCourseInDb = _fitnessServices.DeleteAllCourseGuest(listOfCourseGuest);
                bool statusDeleteGuestToInstructorInDb = _fitnessServices.DeleteAllInstructorGuest(listOfInstructorGuest);

                if (statusDeleteGuestToCourseInDb && statusDeleteGuestToInstructorInDb)
                {
                    TempData["successMessage"] = "Guest deregistered successfully!";
                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                }
                else
                {
                    TempData["errorMessage"] = $"Something went wrong when deleting from database. Guest deregistered from course has status {statusDeleteGuestToCourseInDb} and guest deregistered from instructor has status {statusDeleteGuestToInstructorInDb}";
                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View("GetRegisterGuests", "RegisterGuest");
            }
        }
    }
}
