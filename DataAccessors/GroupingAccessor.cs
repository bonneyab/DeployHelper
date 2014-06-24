using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contract.DTO;
using Core;
using DataAccess.Repository;
using Dependency = DataAccess.Models.Dependency;
using Step = DataAccess.Models.Step;

namespace DataAccessors
{
    public interface IGroupingsAccessor : IDependency
    {
        List<Grouping> GetGroupingsByDeployment(int deploymentId);
        Grouping GetGrouping(int groupingId);
        Grouping CreateGrouping(Grouping grouping);
        void UpdateGrouping(Grouping grouping);
        void DeleteGrouping(int groupingId);
        void ResetGrouping(DataAccess.Models.Grouping grouping);
    }

    public class GroupingsAccessor : IGroupingsAccessor
    {
        private readonly IRepository<DataAccess.Models.Grouping> _groupingsRepository;
        private readonly IRepository<Step> _stepRepository;
        private readonly IRepository<Dependency> _dependencyRepository;

        public GroupingsAccessor(IRepository<DataAccess.Models.Grouping> groupingsRepository, IRepository<Step> stepRepository, IRepository<Dependency> dependencyRepository)
        {
            _groupingsRepository = groupingsRepository;
            _stepRepository = stepRepository;
            _dependencyRepository = dependencyRepository;
        }

        public List<Grouping> GetGroupingsByDeployment(int deploymentId)
        {
            var groupings = _groupingsRepository.FindBy(s => s.DeploymentId == deploymentId);
            return Mapper.Map<List<Grouping>>(groupings);
        }

        public Grouping GetGrouping(int groupingId)
        {
            var grouping = _groupingsRepository.Get(s => s.GroupingId == groupingId);
            return Mapper.Map<Grouping>(grouping);
        }

        public Grouping CreateGrouping(Grouping grouping)
        {
            var createdGrouping = _groupingsRepository.Add(Mapper.Map<DataAccess.Models.Grouping>(grouping));
            _groupingsRepository.Save();
            return Mapper.Map<Grouping>(createdGrouping);
        }

        public void UpdateGrouping(Grouping grouping)
        {
            _groupingsRepository.Edit(Mapper.Map<DataAccess.Models.Grouping>(grouping));
            _groupingsRepository.Save();
        }

        public void DeleteGrouping(int groupingId)
        {
            var grouping = _groupingsRepository.Get(g => g.GroupingId == groupingId);
            var stepsToDelete = _stepRepository.FindBy(s => s.GroupingId == groupingId).ToList();
            var dependenciesToDelete =
                _dependencyRepository.FindBy(
                    d => d.DependentGroupingId == groupingId || d.DependentOnGroupingId == groupingId).ToList();
            stepsToDelete.ForEach(_stepRepository.Delete);
            dependenciesToDelete.ForEach(_dependencyRepository.Delete);
            _groupingsRepository.Delete(grouping);

            //TODO: this seems misleading as all the repositories share the same context so save will actually save all of them.
            _groupingsRepository.Save();
        }

        public void ResetGrouping(DataAccess.Models.Grouping grouping)
        {
            var stepsToReset = _stepRepository.FindBy(s => s.GroupingId == grouping.GroupingId).ToList();
            stepsToReset.ForEach(s => s.Complete = false);
            grouping.Completed = false;
            grouping.VendorActionComplete = false;
            _groupingsRepository.Edit(grouping);
            stepsToReset.ForEach(_stepRepository.Edit);
            _groupingsRepository.Save();
        }
    }
}
