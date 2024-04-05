using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            //get a list of courses that can be assigned to guest <SelectListItem>
            List<SelectListItem> availableCoursesToAssign = _fitnessServices.GetAvailableCoursesRegisterGuest(Id);

            //get a list of all instructors as <SelectListItem>
            List<SelectListItem> allInstructors = _fitnessServices.GetAllInstructorsRegisterGuest();


            //retrieve guest info from db and prepare RegisterGuestViewModel for POST below
            try
            {
                //check if guest with id exists in db
                bool guestExists = _fitnessServices.GuestExists(Id);
                if (!guestExists)
                {
                    TempData["errorMessage"] = $"Guest with the GuestID: {Id} is not found.";
                    return RedirectToAction("GetRegisterGuests", "RegisterGuest");
                }

                //convert and send instructor to POST method
                Guest guest = _fitnessServices.GetGuest(Id);
                RegisterGuestViewModel registerGuestViewModel = new RegisterGuestViewModel()
                {
                    Id = guest.Id,
                    FirstName = guest.FirstName,
                    LastName = guest.LastName,
                    Hours = guest.Hours,
                    AvailableCoursesToAssign = availableCoursesToAssign, //list of strings
                    AllInstructors = allInstructors //list of strings (fName + ' ' + lName)
                };
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
                    return View("EditRegisterGuest", registerGuestViewModel);
                }

                //check if guest was already registered to this course selected in dropdown
                //check if guest was already registered to this instructor selected in dropdown
                bool guestAssignedToCourse = _fitnessServices.CheckGuestAssignedToCourse(registerGuestViewModel.CourseSelected, registerGuestViewModel.Id);
                bool guestAssignedToInstructor = _fitnessServices.CheckGuestAssignedToInstructor(registerGuestViewModel.InstructorSelected, registerGuestViewModel.Id);
                if (guestAssignedToCourse && guestAssignedToInstructor)
                {
                    TempData["errorMessage"] = "Guest is already assigned to this course.";
                    return View("EditRegisterGuest", registerGuestViewModel);
                }

                //check if instructor has free hours equal at least with the hours demanded by the guest
                //************************************* AICI AM RAMAS********************
                //in Services la CheckInstruHasFreeHours sa scot din el si sa faca CalculateInstructorFreeHours
                //si apoi alta metoda: CheckInstructorFreeHours <<-- la Assign Instructor
                //si apoi alta metoda: CheckInstructorFreeHours <<-- la Register Guest 
                // pt ca au formule diferite:
                // instrucHasFreeHours = (assignedHoursForInstructor <= (byte)35) ? true : false;
                // instrucHasFreeHours = (assignedHoursForInstructor + registerGuestViewModel.Hours <= (byte)40) ? true : false;
                bool instrucHasFreeHours = _fitnessServices.CheckInstructorHasFreeHours(registerGuestViewModel.InstructorSelected, (byte)40);
                if (!instrucHasFreeHours)
                {
                    TempData["errorMessage"] = "Instructor does not have enough free hours to take the guest.";
                    return View("EditRegisterGuest", registerGuestViewModel);
                }

                //retrieve the selected course and selected instructor by guest in dropdown (by its name parsed to id)
                Course courseSelected = _fitnessServices.GetCourse(Int32.Parse(registerGuestViewModel.CourseSelected));
                Instructor instructorSelected = _fitnessServices.GetInstructor(Int32.Parse(registerGuestViewModel.InstructorSelected));

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






    }
}
