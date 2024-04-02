using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Repository;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;
using Section = FitnessApp2.Models.DbEntities.Section;

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

        //--------------- RETRIEVE ALL ASSIGNED GUESTS ---------------
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
                    SectionName = currentSection.Name

                };
                guestsViewModel.Add(guestViewModel);
            }

            return View(guestsViewModel);
        }

        //--------------- RETRIEVE ALL UNASSIGNED GUESTS ---------------
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

        //--------------- CREATE A NEW GUEST ---------------

        [HttpGet]
        public IActionResult CreateGuest()
        {
            //get a list of sections that can be assigned to guest
            //List<Section> allSections = _sectionRepository.GetSections().ToList();
            //ViewData["allSections"] = allSections;

            return View();
        }

        [HttpPost]
        public IActionResult CreateGuest(GuestViewModel guestViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var guestNameExists = _guestRepository.GetGuest(guestViewModel.FirstName, guestViewModel.LastName);
                    if (guestNameExists is null)
                    {
                        //find id of chosen Section name
                        Section sectionSelected = _sectionRepository.GetSection(guestViewModel.SectionName);

                        if (sectionSelected is not null)
                        {
                            Guest guest = new Guest()
                            {
                                FirstName = guestViewModel.FirstName,
                                LastName = guestViewModel.LastName,
                                AddedDate = guestViewModel.AddedDate,
                                Hours = guestViewModel.Hours,
                                //DetailId = 
                                SectionId = sectionSelected.Id,
                            };

                            bool statusCreateGuestInDb = _guestRepository.CreateGuest(guest);
                            if (statusCreateGuestInDb)
                            {
                                TempData["successMessage"] = "Instructor created successfully!";
                                return RedirectToAction("GetAssignedGuests", "Guest");
                            }
                            else
                            {
                                TempData["errorMessage"] = "Something went wrong when saving to database.";
                                return View();
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "Invalid Section name. Please choose an existing name.";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Guest already exists.";
                        return View();
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }

            
        }

        //--------------- ASSIGN GUEST TO COURSE AND INSTRUCTOR ---------------
        [HttpGet]
        public IActionResult AssignGuest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AssignGuest(GuestViewModel guestViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check all fields in model to be valid
                    //check instructor name to be present (dropdown)
                    //check course name to be present (dropdown)
                    //check hours to be present

                    //verify chosen instructor has freeHours >= hours from guest
                    //IF FALSE --->>> create new user and add to waitlist
                    //IF TRUE --->>> 
                    //add Section if was provided
                    //add guest to course
                    //add guest to instructor
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

            return View();
        }



    }
}
