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
        [HttpPost]
        public IActionResult CreateInstructor(Instructor instructor)
        {

            return View(instructor);
        }

    }
}
