using System.Linq;
using Core;
using DataAccessors;
using DeploymentHelperServices.Models;

namespace DeploymentHelperServices.ViewModelProviders
{
    public interface IGroupDetailsViewModelProvider : IDependency
    {
        GroupingDetailsViewModel GetGroupDetailsViewModel(int groupingId);
    }

    public class GroupDetailsViewModelProvider : IGroupDetailsViewModelProvider
    {
        private readonly IGroupingsAccessor _groupingsAccessor;
        private readonly IStepAccessor _stepAccessor;
        private readonly IDependencyAccessor _dependencyAccessor;

        public GroupDetailsViewModelProvider(IGroupingsAccessor groupingsAccessor, IStepAccessor stepAccessor, IDependencyAccessor dependencyAccessor)
        {
            _groupingsAccessor = groupingsAccessor;
            _stepAccessor = stepAccessor;
            _dependencyAccessor = dependencyAccessor;
        }

        public GroupingDetailsViewModel GetGroupDetailsViewModel(int groupingId)
        {
            //TODO: 4 queries...one of which is really redundant...
            //TODO: there's a lot of interesting things you could do around caching.
            var grouping = _groupingsAccessor.GetGrouping(groupingId);
            var steps = _stepAccessor.GetStepsByGrouping(grouping.GroupingId);
            var dependencies = _dependencyAccessor.GetDependenciesByGrouping(groupingId);
            var availableDependencies = _dependencyAccessor.GetAvailableDependenciesByDeployment(grouping.DeploymentId);

            availableDependencies =
                availableDependencies.Where(
                    d =>
                        !dependencies.Select(dep => dep.GroupingId).Contains(d.GroupingId) && d.GroupingId != groupingId).ToList();

            return new GroupingDetailsViewModel
            {
                Group = grouping,
                Steps = steps,
                Dependencies = dependencies,
                AvailableDependencies = availableDependencies
            };
        }
    }
}
