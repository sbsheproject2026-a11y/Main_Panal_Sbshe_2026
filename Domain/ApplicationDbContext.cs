using Microsoft.EntityFrameworkCore;

using Model;

namespace Domain
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MasterType> MasterType { get; set; }
        public DbSet<Lecture> Lecture { get; set; }
        public DbSet<MasterTypeDetail> MasterTypeDetail { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<MasterLogin> MasterLogin { get; set; }
        public DbSet<Entity> Entity { get; set; }
        public DbSet<BasicProfile> BasicProfile { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<MasterSession> MasterSession { get; set; }
        public DbSet<StudentMarks> StudentMarks { get; set; }
        public DbSet<EntityUserResult> EntityUserResult { get; set; }
        public DbSet<EmployeeFrenchiseAssign> EmployeeFrenchiseAssign { get; set; }
        public DbSet<AcademicDetails> AcademicDetails { get; set; }
        public DbSet<Enquiries> Enquiries { get; set; }

    }
}



