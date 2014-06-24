using System.Linq;
using Contract.DTO;
using Core;
using DataAccessors;
using DeploymentHelperServices.Models;

namespace DeploymentHelperServices.ViewModelProviders
{
    public interface IDeploymentDetailsViewModelProvider : IDependency
    {
        DeploymentDetailsViewModel GetDeploymentDetailsViewModel(int deploymentId);
    }

    public class DeploymentDetailsViewModelProvider : IDeploymentDetailsViewModelProvider
    {
        private readonly IDeploymentAccessor _deploymentAccessor;
        private readonly IGroupingsAccessor _groupingsAccessor;
        private readonly IDependencyAccessor _dependencyAccessor;
        private readonly IStepAccessor _stepAccessor;

        public DeploymentDetailsViewModelProvider(IDeploymentAccessor deploymentAccessor,
            IGroupingsAccessor groupingsAccessor, IDependencyAccessor dependencyAccessor,
            IStepAccessor stepAccessor)
        {
            _deploymentAccessor = deploymentAccessor;
            _groupingsAccessor = groupingsAccessor;
            _dependencyAccessor = dependencyAccessor;
            _stepAccessor = stepAccessor;
        }

        public DeploymentDetailsViewModel GetDeploymentDetailsViewModel(int deploymentId)
        {
            var groupings = _groupingsAccessor.GetGroupingsByDeployment(deploymentId)
                .OrderByDescending(o => o.Weight).ToList();

            //TODO: Figure out how to use automapper for this?
            var groupingModels = groupings.Select(g => new GroupViewModel
            {
                Status = GetStatus(g),
                DeploymentId = g.DeploymentId,
                GroupingId = g.GroupingId,
                Name = g.Name,
                Owner = g.Owner,
                VendorActionComplete = g.VendorActionComplete,
                VendorActionRequired = g.VendorActionRequired,
                Weight = g.Weight,
                CssClass = GetGroupingCssClass(g)
            });
            return new DeploymentDetailsViewModel
            {
                Deployment = _deploymentAccessor.GetDeployment(deploymentId),
                Groupings = groupingModels
            };
        }

        //TODO: this really feels a lot like code duplication...use a tuple?
        public string GetStatus(Grouping grouping)
        {
            if (grouping.Completed) return "Complete";

            var inProgress = _stepAccessor.GetStepsByGrouping(grouping.GroupingId).Any(s => s.Complete);
            if (inProgress) return "In Progress";

            var notReady = _dependencyAccessor.GetDependenciesByGrouping(grouping.GroupingId).Any(g => !g.Completed);
            return notReady ? "Not Ready" : "Ready";
        }

        public string GetGroupingCssClass(Grouping grouping)
        {
            if (grouping.Completed) return "success";
            var inProgress = _stepAccessor.GetStepsByGrouping(grouping.GroupingId).Any(s => s.Complete);
            if (inProgress) return "info";

            var notReady = _dependencyAccessor.GetDependenciesByGrouping(grouping.GroupingId).Any(g => !g.Completed);
            return notReady ? "danger" : "";
        }
    }
}
