using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp2.Controllers
{
    public class GuestController : Controller
    {
        private readonly IGuestRepository _guestRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IDetailRepository _detailRepository;

        public GuestController(
            IGuestRepository guestRepository, 
            ISectionRepository sectionRepository, 
            IInstructorRepository instructorRepository,
            IDetailRepository detailRepository
            )
        {
            this._guestRepository = guestRepository;
            this._sectionRepository = sectionRepository;
            this._instructorRepository = instructorRepository;
            this._detailRepository = detailRepository;
        }

        public IInstructorRepository InstructorRepository { get; }

        public IActionResult GetAssignedGuests()
        {
            ICollection<Guest> guests = _guestRepository.GetAssignedGuests();
            List<Guest> guestsAsList = guests.ToList();

            List<GuestViewModel> guestsViewModel = new List<GuestViewModel>();

            foreach (Guest guest in guestsAsList)
            {
                Section currentSection = _sectionRepository.GetSection((int)guest.SectionId);
                //Instructor currentInstructor = _instructorRepository.GetInstructor((int)guest.InstructorId);
                GuestViewModel guestViewModel = new GuestViewModel()
                {
                    Id = guest.Id,
                    FirstName = guest.FirstName,
                    LastName = guest.LastName,
                    AddedDate = guest.AddedDate,
                    Hours = guest.Hours,
                    Section = currentSection.Name,
                    //Instructor = currentInstructor.FirstName + " " + currentInstructor.LastName,

                };
                guestsViewModel.Add(guestViewModel);
            }

            return View(guestsViewModel);
        }

        public IActionResult GetUnassignedGuests()
        {
            ICollection<Guest> guests = _guestRepository.GetUnassignedGuests();
            List<Guest> guestsAsList = guests.ToList();

            List<WaitlistGuestViewModel> waitlistGuestsViewModel = new List<WaitlistGuestViewModel>();

            foreach (Guest guest in guestsAsList)
            {
                Detail currentDetail = _detailRepository.GetDetail(guest.DetailId);
                WaitlistGuestViewModel waitlistGuestViewModel = new WaitlistGuestViewModel()
                {
                    Id = guest.Id,
                    FirstName = guest.FirstName,
                    LastName = guest.LastName,
                    Hours = guest.Hours,
                    Email = currentDetail.Email,
                    Phone = currentDetail.Phone,

                };
                waitlistGuestsViewModel.Add(waitlistGuestViewModel);
            }

            return View(waitlistGuestsViewModel);
        }
    }
}
