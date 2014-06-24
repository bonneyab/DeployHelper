using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DataAccess.Models;

namespace DataAccess.DataAccessLayer
{
    public class DeployHelperContext : DbContext
    {
        public DeployHelperContext()
            : base("DeployHelperContext")
        {
        }

        public DbSet<Dependency> Dependencies { get; set; }
        public DbSet<Deployment> Deployments { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Grouping> Groupings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
