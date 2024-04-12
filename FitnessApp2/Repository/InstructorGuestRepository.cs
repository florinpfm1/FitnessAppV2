using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp2.Repository
{
    public class InstructorGuestRepository : IInstructorGuestRepository
    {
        private readonly FAppDbContext _context;

        public InstructorGuestRepository(FAppDbContext context)
        {
            this._context = context;
        }

        //retrieving
        public ICollection<InstructorGuest> GetGuestsByInstructorId(int instrucId)
        {
            return _context.InstructorGuests.Where(g => g.InstructorId == instrucId).ToList();
        }

        public ICollection<InstructorGuest> GetInstructorsByGuestId(int guestId)
        {
            return _context.InstructorGuests.Where(i => i.GuestId == guestId).ToList();
        }

        public InstructorGuest GetInstructorGuestByInstructorIdAndGuestId(int instrucId, int guestId)
        {
            return _context.InstructorGuests.Where(x => x.InstructorId == instrucId && x.GuestId == guestId).FirstOrDefault();
        }

        public bool GuestHasInstructors(int instrucId)
        {
            return _context.InstructorGuests.Any(g => g.InstructorId == instrucId);
        }

        //checking
        public bool InstructorHasGuests(int guestId)
        {
            return _context.InstructorGuests.Any(i => i.GuestId == guestId);
        }

        //assign a guest to an instructor
        public bool RegisterGuest(InstructorGuest instructorGuest)
        {
            _context.Add(instructorGuest);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); 
            return saved > 0 ? true : false;
        }

        //deleting all existing instructor<->guest
        public bool DeleteAllInstructorGuest(List<InstructorGuest> listOfInstructorGuest)
        {
            foreach (InstructorGuest instructorGuest in listOfInstructorGuest)
            {
                _context.Remove(instructorGuest);
            }
            return Save();
        }

        //deleting one guest from a instructor<->guest
        public bool DeleteInstructorGuest(InstructorGuest instructorGuest)
        {
            _context.Remove(instructorGuest);
            return Save();
        }
    }
}
