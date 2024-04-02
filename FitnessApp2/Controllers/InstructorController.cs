using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp2.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorController(IInstructorRepository instructorRepository)
        {
            this._instructorRepository = instructorRepository;
        }

        //--------------- RETRIEVE ALL INSTRUCTORS ---------------
        [HttpGet]
        public IActionResult GetInstructors()
        {
            ICollection<Instructor> instructors = _instructorRepository.GetInstructors();
            List<Instructor> instructorsAsList = instructors.ToList();
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

        //--------------- CREATE A NEW INSTRUCTORS ---------------
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
                if (ModelState.IsValid)
                {
                    var instructorNameExists = _instructorRepository.GetInstructor(instructorViewModel.FirstName, instructorViewModel.LastName);
                    if (instructorNameExists != null)
                    {
                        Instructor instructor = new Instructor()
                        {
                            FirstName = instructorViewModel.FirstName,
                            LastName = instructorViewModel.LastName,
                            AddedDate = instructorViewModel.AddedDate,
                            ExperienceYears = instructorViewModel.ExperienceYears,
                            Gender = instructorViewModel.Gender
                        };

                        bool statusCreateInstructorInDb = _instructorRepository.CreateInstructor(instructor);
                        if (statusCreateInstructorInDb)
                        {
                            TempData["successMessage"] = "Instructor created successfully!";
                            return RedirectToAction("GetInstructors", "Instructor");
                        }
                        else
                        {
                            TempData["errorMessage"] = "Something went wrong when saving to database.";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Instructor already exists.";
                        return View();
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View();
                }
            } catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }






    }
}
