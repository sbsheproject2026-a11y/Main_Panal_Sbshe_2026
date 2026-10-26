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

        public DbSet<RTIApplications> RTIApplications { get; set; }
        public DbSet<RTIDocuments> RTIDocuments { get; set; }
        public DbSet<MasterType> MasterType { get; set; }
        public DbSet<CommissionAmount> CommissionAmount { get; set; }
        public DbSet<Lecture> Lecture { get; set; }
        public DbSet<MasterTypeDetail> MasterTypeDetail { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<MasterLogin> MasterLogin { get; set; }
        public DbSet<ProductAmount> ProductAmount { get; set; }
        public DbSet<Entity> Entity { get; set; }
        public DbSet<BasicProfile> BasicProfile { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<MasterSession> MasterSession { get; set; }
        public DbSet<StudentMarks> StudentMarks { get; set; }
        public DbSet<EntityWalletResult> EntityWalletResult { get; set; }
        //public DbSet<DashboardStatsDTO> DashboardStatsDTO { get; set; }
        public DbSet<EntityUserResult> EntityUserResult { get; set; }
        public DbSet<EmployeeFrenchiseAssign> EmployeeFrenchiseAssign { get; set; }
        public DbSet<AcademicDetails> AcademicDetails { get; set; }
        public DbSet<Enquiries> Enquiries { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductCategoryDetail> ProductCategoryDetail { get; set; }
        public DbSet<ProductCategoryDocument> ProductCategoryDocument { get; set; }
        public DbSet<ProductPersonalDetail> ProductPersonalDetail { get; set; }
        public DbSet<ProductLink> ProductLink { get; set; }
        public DbSet<Commission> Commission { get; set; }
        public DbSet<FranchiseCommission> FranchiseCommission { get; set; }
        public DbSet<AddressProfile> AddressProfile { get; set; }

    }
}



