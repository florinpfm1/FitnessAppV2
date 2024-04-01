﻿using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Models.DbEntities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FitnessApp2.Repository
{
    public class InstructorRepository : IInstructorRepository //repository will inherit from the interface
    {
        //add data context constructor and property to manipulate all tables from db
        public FAppDbContext _context { get; }
        public InstructorRepository(FAppDbContext context)
        {
           this._context = context;
        }

        //get a List of all instructors
        //because we want output an ICollection we need to add the ToList() at the end and be specific of what we should return from the db 
        public ICollection<Instructor> GetInstructors()
        {
            return _context.Instructors.OrderBy(i => i.Id).ToList(); 
        }

        //get one instructor by Id
        //because we want output an object Instructor we need to add FirstOrDefault() at the end and be specific of what we should return from the db
        public Instructor GetInstructor(int id)
        {
            return _context.Instructors.Where(i => i.Id == id).FirstOrDefault();
        }

        

        //get one instructor by FirstName and LastName
        public Instructor GetInstructor(string firstName, string lastName)
        {
            return _context.Instructors.Where(i => i.FirstName == firstName && i.LastName == lastName).FirstOrDefault();
        }

        public ICollection<Instructor> GetInstructors(DateOnly date)
        {
            throw new NotImplementedException();
        }

        //get a List of all instructors by ExperienceYears
        public ICollection<Instructor> GetInstructors(byte expYears)
        {
            return _context.Instructors.Where(i => i.ExperienceYears == expYears).OrderBy(i => i.Id).ToList();
        }

        //get a List of all instructors by Gender
        public ICollection<Instructor> GetInstructors(char gender)
        {
            return _context.Instructors.Where(i => i.Gender == gender).OrderBy(i => i.Id).ToList();
        }

        //checks if an instructors exists in db with that Id
        //Any() will return a bool true/false
        public bool InstructorExists(int instrucId)
        {
            return _context.Instructors.Any(i => i.Id == instrucId);
        }

        //creating a new instructor
        public bool CreateInstructor(Instructor instructor)
        {
            //change tracker
            //add, updating, modifying
            //connected vs disconnected   (90% we will work in connected state)
            //EntityState.Added = ...     (is a disconnected state)
            _context.Add(instructor);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); //SaveChanges() returns an integer ; is the equivalent of _context.SaveChanges() put inside the CreateInstructor(...) method
            return saved > 0 ? true : false;

            //when we call SaveChanges() then Entity framework will take all we placed in _context, convert it into SQL and send it to db
        }
    }
}