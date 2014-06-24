using System.Collections.Generic;
using System.Data.Entity;
using DataAccess.Models;

namespace DataAccess.DataAccessLayer
{
    //TODO: I'm really not actually a fan of the Code first approach, I'm far too controlling of my databases, but it seems to make sense for something I want anyone to be able to run.
    public class DeployHelperInitializer : DropCreateDatabaseIfModelChanges<DeployHelperContext>
    {
        protected override void Seed(DeployHelperContext context)
        {
            var deployment = new Deployment
            {
                Name = "FSI Deployment"
            };
            context.Deployments.Add(deployment);
            context.SaveChanges();

            var groupings = new List<Grouping>
            {
                new Grouping {Name = "HCMS", DeploymentId = 1},
                new Grouping {Name = "Workday", DeploymentId = 1}
            };
            groupings.ForEach(p => context.Groupings.Add(p));
            context.SaveChanges();

            var steps = new List<Step>
            {
                new Step {GroupingId = 1, Description = "Step 1", Order = 1},
                new Step {GroupingId = 1, Description = "Step 2", Order = 2},
                new Step {GroupingId = 2, Description = "Step 1", Order = 1}
            };
            steps.ForEach(p => context.Steps.Add(p));
            context.SaveChanges();

            var dependencies = new List<Dependency>
            {
                new Dependency {DependentOnGroupingId = 1, DependentGroupingId = 2}
            };
            dependencies.ForEach(p => context.Dependencies.Add(p));
            context.SaveChanges();
        }
    }
}
