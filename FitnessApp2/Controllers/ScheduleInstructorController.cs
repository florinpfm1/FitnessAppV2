using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Repository;
using FitnessApp2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace FitnessApp2.Controllers
{
    public class ScheduleInstructorController : Controller
    {
        private readonly IFitnessServices _fitnessServices;

        public ScheduleInstructorController(IFitnessServices fitnessServices)
        {
            this._fitnessServices = fitnessServices;
        }

        [HttpGet]
        public IActionResult GetScheduleInstructors()
        {
            List<ScheduleInstructorViewModel> scheduleInstructorsViewModel = new List<ScheduleInstructorViewModel>();
            scheduleInstructorsViewModel = _fitnessServices.BuildScheduleForInstructors().ToList();

            return View(scheduleInstructorsViewModel);
        }
    }
}
