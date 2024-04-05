using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Repository;
using FitnessApp2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

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
            //get a list of course that can be assigned to instructor
            List<SelectListItem> availableCoursesToAssign = _fitnessServices.GetAvailableCoursesAssignInstruc(Id);

            //retrieve instructor info from db and prepare AssignInstructorViewModel for POST below
            try
            {
                //check if instructor with id exists in db
                bool instructorExists = _fitnessServices.InstructorExists(Id);
                if (!instructorExists)
                {
                    TempData["errorMessage"] = $"Instructor with the InstructorID: {Id} is not found.";
                    return RedirectToAction("GetAssignInstructors", "AssignInstructor");
                }

                //convert and send instructor to POST method
                Instructor instructor = _fitnessServices.GetInstructor(Id);
                AssignInstructorViewModel assignInstructorViewModel = new AssignInstructorViewModel()
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    AvailableCoursesToAssign = availableCoursesToAssign, //list of strings
                };
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
                    return View(assignInstructorViewModel);
                }

                //check if instructor has at least 5 hours free to take on one more guest (which can choose between 1...5 hours)
                bool instrucHasFreeHours = _fitnessServices.CheckInstructorHasFreeHours(assignInstructorViewModel.Id, (byte)35);
                if (!instrucHasFreeHours) 
                {
                    TempData["errorMessage"] = "Instructor does not have free hours to start a new course.";
                    return View(assignInstructorViewModel);
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

    }
}
