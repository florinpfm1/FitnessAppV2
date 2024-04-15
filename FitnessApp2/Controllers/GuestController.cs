using FitnessApp2.Models.DbEntities;
using FitnessApp2.Models.ViewModels;
using FitnessApp2.Services;
using Microsoft.AspNetCore.Mvc;
//using Section = FitnessApp2.Models.DbEntities.Section;

namespace FitnessApp2.Controllers
{
    public class GuestController : Controller
    {
        private readonly IFitnessServices _fitnessServices;
        public GuestController(IFitnessServices fitnessServices)
        {
            this._fitnessServices = fitnessServices;
        }

        //--------------- RETRIEVE ALL ASSIGNED GUESTS ---------------
        public IActionResult GetAssignedGuests()
        {
            ICollection<Guest> guestsAsList = _fitnessServices.GetAssignedGuests();
            List<GuestViewModel> guestsViewModel = new List<GuestViewModel>();

            foreach (Guest guest in guestsAsList)
            {
                Section currentSection = _fitnessServices.GetSection((int)guest.SectionId);
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
            ICollection<Guest> guestsAsList = _fitnessServices.GetUnassignedGuests();
            List<WaitlistGuestViewModel> waitlistGuestsViewModel = new List<WaitlistGuestViewModel>();

            foreach (Guest guest in guestsAsList)
            {
                Detail currentDetail = _fitnessServices.GetDetail(guest.DetailId);
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
            return View();
        }

        [HttpPost]
        public IActionResult CreateGuest(GuestViewModel guestViewModel)
        {
            try
            {
                //check model
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View();
                }

                //check if guest with this name is already added
                Guest guestNameExists = _fitnessServices.GetGuest(guestViewModel.FirstName, guestViewModel.LastName);
                if (guestNameExists != null)
                {
                    {
                        TempData["errorMessage"] = "Guest already exists.";
                        return View();
                    }
                }

                //check chosen Section name
                Section sectionSelected = _fitnessServices.GetSection(guestViewModel.SectionName);

                if (sectionSelected is null)
                {
                    TempData["errorMessage"] = "Invalid Section name. Please choose an existing name.";
                    return View();
                }

                //check if ANY INSTRUCTOR has at least 5 hours free to take on one more course with at least 1...5 guests (each guest can have between 1...5 hours)
                ICollection<Instructor> instructorsAsList = _fitnessServices.GetInstructors().ToList();
                bool anyInstructorHasFreeHours = false;
                foreach (Instructor instructor in instructorsAsList)
                {
                    bool instrucHasFreeHours = _fitnessServices.CheckInstructorHasFreeHours(instructor.Id, (byte)35, "forWaitlistGuest", guestViewModel.Hours);
                    if (instrucHasFreeHours)
                    {
                        anyInstructorHasFreeHours = true;
                        break;
                    }
                }
                if (!anyInstructorHasFreeHours)
                {
                    //return view to fill in email and phone
                    WaitlistGuestViewModel model = new WaitlistGuestViewModel();
                    TempData["errorMessage"] = "No Instructors does are available to new guests. You will be redirected to the Waitlist.";
                    return View("CreateUnassignedGuest", model);
                }

                //add and save guest to db context
                Guest guest = new Guest()
                {
                    FirstName = guestViewModel.FirstName,
                    LastName = guestViewModel.LastName,
                    AddedDate = guestViewModel.AddedDate,
                    Hours = guestViewModel.Hours,
                    SectionId = sectionSelected.Id,
                };

                bool statusCreateGuestInDb = _fitnessServices.CreateGuest(guest);
                if (statusCreateGuestInDb)
                {
                    TempData["successMessage"] = "Instructor created successfully!";
                    return RedirectToAction("GetAssignedGuests", "Guest");
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong when saving to database.";
                    return RedirectToAction("GetAssignedGuests", "Guest");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        //--------------- EDIT A GUEST ---------------
        [HttpGet]
        public IActionResult EditGuest(int Id)
        {
            try
            {
                //check if guest with id exists in db
                bool guestExists = _fitnessServices.GuestExists(Id);
                if (!guestExists)
                {
                    TempData["errorMessage"] = $"Guest details not available with the GuestID: {Id}";
                    return RedirectToAction("GetAssignedGuests", "Guest");
                }

                //retrieve Section name of the guest
                Guest guest = _fitnessServices.GetGuest(Id);
                Section sectionSelected = _fitnessServices.GetSection(guest.SectionId);

                //convert and send instructor to POST method
                GuestViewModel guestViewModel = new GuestViewModel()
                {
                    Id = guest.Id,
                    FirstName = guest.FirstName,
                    LastName = guest.LastName,
                    AddedDate = guest.AddedDate,
                    Hours = guest.Hours,
                    SectionName = sectionSelected.Name
                };
                return View(guestViewModel);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetAssignedGuests", "Guest");
            }
        }

        [HttpPost]
        public IActionResult EditGuest(GuestViewModel guestViewModel)
        {
            try
            {
                //check model
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View();
                }

                //check if guest with this name is already added
                Guest guestNameExists = _fitnessServices.GetGuest(guestViewModel.FirstName, guestViewModel.LastName);
                if (guestNameExists != null)
                {
                    {
                        TempData["errorMessage"] = "Guest already exists.";
                        return View();
                    }
                }

                //check chosen Section name
                Section sectionSelected = _fitnessServices.GetSection(guestViewModel.SectionName);

                if (sectionSelected is null)
                {
                    TempData["errorMessage"] = "Invalid Section name. Please choose an existing name.";
                    return View();
                }

                //convert model,update and save guest to db context
                Guest guest = _fitnessServices.GetGuest(guestViewModel.Id);
                guest.FirstName = guestViewModel.FirstName;
                guest.LastName = guestViewModel.LastName;
                guest.AddedDate = guestViewModel.AddedDate;
                guest.Hours = guestViewModel.Hours;
                guest.SectionId = sectionSelected.Id;

                bool statusUpdateGuestInDb = _fitnessServices.UpdateGuest(guest);
                if (statusUpdateGuestInDb)
                {
                    TempData["successMessage"] = "Guest updated successfully!";
                    return RedirectToAction("GetAssignedGuests", "Guest");
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong when updating to database.";
                    return RedirectToAction("GetAssignedGuests", "Guest");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        //--------------- DELETE A GUEST ---------------
        [HttpGet]
        public IActionResult DeleteGuest(int Id)
        {
            try
            {
                //check if guest with id exists in db
                bool guestExists = _fitnessServices.GuestExists(Id);
                if (!guestExists)
                {
                    TempData["errorMessage"] = $"Guest details not available with the GuestID: {Id}";
                    return RedirectToAction("GetAssignedGuests", "Guest");
                }

                //retrieve Section name of the guest
                Guest guest = _fitnessServices.GetGuest(Id);
                Section sectionSelected = _fitnessServices.GetSection(guest.SectionId);

                //convert and send guest to POST method
                GuestViewModel guestViewModel = new GuestViewModel()
                {
                    Id = guest.Id,
                    FirstName = guest.FirstName,
                    LastName = guest.LastName,
                    AddedDate = guest.AddedDate,
                    Hours = guest.Hours,
                    SectionName = sectionSelected.Name
                };
                return View(guestViewModel);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetAssignedGuests", "Guest");
            }
        }

        [HttpPost]
        public IActionResult DeleteGuest(GuestViewModel guestViewModel)
        {
            try
            {
                //delete and save guest to db context
                Guest guest = _fitnessServices.GetGuest(guestViewModel.Id);

                bool statusDeleteGuestInDb = _fitnessServices.DeleteGuest(guest);
                if (statusDeleteGuestInDb)
                {
                    TempData["successMessage"] = "Guest deleted successfully!";
                    return RedirectToAction("GetInstructors", "Instructor");
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong when deleting in database.";
                    return RedirectToAction("GetAssignedGuests", "Guest");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetAssignedGuests", "Guest");
            }
        }

        //--------------- CREATE NEW GUEST ON WAITLIST ---------------
        [HttpPost]
        public IActionResult CreateUnassignedGuest(WaitlistGuestViewModel waitlistGuestViewModel)
        {
            try
            {
                //check model
                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is not valid.";
                    return View();
                }

                //check if detail with this email and phone is already added to waitlist
                Detail detailExists = _fitnessServices.GetDetailByPhoneAndEmail(waitlistGuestViewModel.Email, waitlistGuestViewModel.Phone);
                if (detailExists != null)
                {
                    {
                        TempData["errorMessage"] = "Guest alread added to Waitlist.";
                        return View();
                    }
                }

                //add and save guest to db context - this is a waitlist guest and will have DetailId
                // --create detail for guest
                Detail detail = new Detail()
                {
                    Email = waitlistGuestViewModel.Email,
                    Phone = waitlistGuestViewModel.Phone
                };
                bool statusCreateDetailInDb = _fitnessServices.CreateDetail(detail);

                // --retrieve detail id
                Detail currentDetail = _fitnessServices.GetDetailByPhoneAndEmail(detail.Email, detail.Phone);

                // --add and save guest to db context
                Guest guest = new Guest()
                {
                    FirstName = waitlistGuestViewModel.FirstName,
                    LastName = waitlistGuestViewModel.LastName,
                    Hours = waitlistGuestViewModel.Hours,
                    DetailId = currentDetail.Id
                };
                bool statusCreateGuestInDb = _fitnessServices.CreateGuest(guest);
                if (statusCreateDetailInDb && statusCreateGuestInDb)
                {
                    TempData["successMessage"] = "Guest added to Waitlist successfully!";
                    return RedirectToAction("GetAssignedGuests", "Guest");
                }
                else
                {
                    TempData["errorMessage"] = $"Something went wrong when saving to database. Detail for guest has status {statusCreateDetailInDb} and guest added to Waitlist has status {statusCreateGuestInDb}";
                    TempData["errorMessage"] = "Something went wrong when saving to database.";
                    return RedirectToAction("GetAssignedGuests", "Guest");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
