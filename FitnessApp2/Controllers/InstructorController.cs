using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Repository;
using FitnessApp2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp2.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IFitnessServices _fitnessServices;
        public InstructorController(IFitnessServices fitnessServices)
        {
            this._fitnessServices = fitnessServices;
        }

        //--------------- RETRIEVE ALL INSTRUCTORS ---------------
        [HttpGet]
        public IActionResult GetInstructors()
        {
            ICollection<Instructor> instructorsAsList = _fitnessServices.GetInstructors();
            List<InstructorViewModel> instructorsViewModel = new List<InstructorViewModel>();

            foreach (Instructor instructor in instructorsAsList)
            {
                InstructorViewModel instructorViewModel = new InstructorViewModel()
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    AddedDate = (instructor.AddedDate is not null) ? instructor.AddedDate : null,
                    ExperienceYears = (instructor.ExperienceYears is not null) ? instructor.ExperienceYears : (byte)0,
                    Gender = (instructor.Gender is not null) ? instructor.Gender : null
                };
                instructorsViewModel.Add(instructorViewModel);
            }
            return View(instructorsViewModel);
        }

        //--------------- CREATE A NEW INSTRUCTOR ---------------
        [HttpGet]
        public IActionResult CreateInstructor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateInstructor(InstructorViewModel instructorViewModel)
        {
            try
            {
                //check model
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View();
                }

                //check if instructor with this name is already added
                Instructor instructorNameExists = _fitnessServices.GetInstructor(instructorViewModel.FirstName, instructorViewModel.LastName);
                if (instructorNameExists != null)
                {
                    TempData["errorMessage"] = "Instructor already exists.";
                    return View();
                }

                //add and save instructor to db context
                Instructor instructor = new Instructor()
                {
                    FirstName = instructorViewModel.FirstName,
                    LastName = instructorViewModel.LastName,
                    AddedDate = instructorViewModel.AddedDate,
                    ExperienceYears = instructorViewModel.ExperienceYears,
                    Gender = instructorViewModel.Gender
                };

                bool statusCreateInstructorInDb = _fitnessServices.CreateInstructor(instructor);
                if (statusCreateInstructorInDb)
                {
                    TempData["successMessage"] = "Instructor created successfully!";
                    return RedirectToAction("GetInstructors", "Instructor");
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong when saving to database.";
                    return RedirectToAction("GetInstructors", "Instructor");
                }
            } 
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        //--------------- EDIT AN INSTRUCTOR ---------------
        [HttpGet]
        public IActionResult EditInstructor(int Id)
        {
            try
            {
                //check if instructor with id exists in db
                bool instructorExists = _fitnessServices.InstructorExists(Id);
                if (!instructorExists)
                {
                    TempData["errorMessage"] = $"Instructor details not available with the InstructorID: {Id}";
                    return RedirectToAction("GetInstructors", "Instructor");
                }

                //convert and send instructor to PUT method
                Instructor instructor = _fitnessServices.GetInstructor(Id);
                InstructorViewModel instructorViewModel = new InstructorViewModel()  
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    AddedDate = instructor.AddedDate,
                    ExperienceYears = instructor.ExperienceYears,
                    Gender = instructor.Gender
                };
                return View(instructorViewModel);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetInstructors", "Instructor");
            }
        }

        [HttpPost]
        public IActionResult EditInstructor(InstructorViewModel instructorViewModel)
        {
            try
            {
                //check model
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View();
                }

                //convert model,update and save instructor to db context
                Instructor instructor = _fitnessServices.GetInstructor(instructorViewModel.Id);
                instructor.FirstName = instructorViewModel.FirstName;
                instructor.LastName = instructorViewModel.LastName;
                instructor.AddedDate = instructorViewModel.AddedDate;
                instructor.ExperienceYears = instructorViewModel.ExperienceYears;
                instructor.Gender = instructorViewModel.Gender;

                bool statusUpdateInstructorInDb = _fitnessServices.UpdateInstructor(instructor);
                if (statusUpdateInstructorInDb)
                {
                    TempData["successMessage"] = "Instructor updated successfully!";
                    return RedirectToAction("GetInstructors", "Instructor");
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong when updating to database.";
                    return RedirectToAction("GetInstructors", "Instructor");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        //--------------- DELETE AN INSTRUCTOR ---------------
        [HttpGet]
        public IActionResult DeleteInstructor(int Id)
        {
            try
            {
                //check if instructor with id exists in db
                bool instructorExists = _fitnessServices.InstructorExists(Id);
                if (!instructorExists)
                {
                    TempData["errorMessage"] = $"Instructor details not available with the InstructorID: {Id}";
                    return RedirectToAction("GetInstructors", "Instructor");
                }

                //convert and send instructor to DELETE method
                Instructor instructor = _fitnessServices.GetInstructor(Id);
                InstructorViewModel instructorViewModel = new InstructorViewModel()
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    AddedDate = instructor.AddedDate,
                    ExperienceYears = instructor.ExperienceYears,
                    Gender = instructor.Gender
                };
                return View(instructorViewModel);

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetInstructors", "Instructor");
            }
        }

        [HttpPost]
        public IActionResult DeleteInstructor(InstructorViewModel instructorViewModel)
        {
            try
            {
                //delete and save instructor to db context
                Instructor instructor = _fitnessServices.GetInstructor(instructorViewModel.Id);

                bool statusDeleteInstructorInDb = _fitnessServices.DeleteInstructor(instructor);
                if (statusDeleteInstructorInDb)
                {
                    TempData["successMessage"] = "Instructor deleted successfully!";
                    return RedirectToAction("GetInstructors", "Instructor");
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong when deleting in database.";
                    return RedirectToAction("GetInstructors", "Instructor");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetInstructors", "Instructor");
            }
        }
    }
}
