using FitnessApp2.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp2.Data
{
    public class FAppDbContext : DbContext
    {
        //---Constructor---
        public FAppDbContext(DbContextOptions<FAppDbContext> options) : base(options)
        {
        }

        //---Properties---
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseInstructor> CourseInstructors { get; set; }
        public DbSet<CourseGuest> CourseGuests { get; set; }

        //---in case of many-to-many relationships for SQL tables here we need to customize the links of the FK's together:---
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseInstructor>()
                .HasKey(ci => new { ci.CourseId, ci.InstructorId });
            modelBuilder.Entity<CourseInstructor>()
                .HasOne(c => c.Course)
                .WithMany(ci => ci.CourseInstructors)
                .HasForeignKey(c => c.CourseId);
            modelBuilder.Entity<CourseInstructor>()
                .HasOne(i => i.Instructor)
                .WithMany(ci => ci.CourseInstructors)
                .HasForeignKey(i => i.InstructorId);

            modelBuilder.Entity<CourseGuest>()
                .HasKey(cg => new { cg.CourseId, cg.GuestId });
            modelBuilder.Entity<CourseGuest>()
                .HasOne(c => c.Course)
                .WithMany(cg => cg.CourseGuests)
                .HasForeignKey(c => c.CourseId);
            modelBuilder.Entity<CourseGuest>()
                .HasOne(g => g.Guest)
                .WithMany(cg => cg.CourseGuests)
                .HasForeignKey(g => g.GuestId);

        }
    }
}
