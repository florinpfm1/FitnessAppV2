using FitnessApp2.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp2.Data
{
    public class FApplicationDbContext : DbContext
    {
        //---Constructor---
        public FApplicationDbContext(DbContextOptions<FApplicationDbContext> options) : base(options)
        {
        }

        //---Properties---
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PossibleClient> PossibleClients { get; set; }
    }
}
