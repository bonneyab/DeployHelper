using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contract.DTO;
using Core;
using DataAccess.Repository;

namespace DataAccessors
{
    public interface IDependencyAccessor : IDependency
    {
        List<Grouping> GetDependenciesByGrouping(int groupingId);
        Grouping CreateDependency(Dependency dependency);
        List<Grouping> GetAvailableDependenciesByDeployment(int deploymentId);
        void DeleteDependency(int dependentOnId, int dependentId);
    }

    public class DependencyAccessor : IDependencyAccessor
    {
        private readonly IRepository<DataAccess.Models.Dependency> _dependencyRepository;
        private readonly IRepository<DataAccess.Models.Grouping> _groupingRepository;

        public DependencyAccessor(IRepository<DataAccess.Models.Dependency> dependencyRepository, 
            IRepository<DataAccess.Models.Grouping> groupingRepository)
        {
            _dependencyRepository = dependencyRepository;
            _groupingRepository = groupingRepository;
        }

        public List<Grouping> GetDependenciesByGrouping(int groupingId)
        {
            var dependencies = (from d in _dependencyRepository.GetAll()
                                    join g in _groupingRepository.GetAll() on d.DependentGroupingId equals g.GroupingId
                                    join gr in _groupingRepository.GetAll() on d.DependentOnGroupingId equals gr.GroupingId
                                    where d.DependentGroupingId == groupingId
                                    select gr);
            return Mapper.Map<List<Grouping>>(dependencies);
        }

        public List<Grouping> GetAvailableDependenciesByDeployment(int deploymentId)
        {
            var availableDependencies = _groupingRepository.FindBy(g => g.DeploymentId == deploymentId);
            return Mapper.Map<List<Grouping>>(availableDependencies);
        }

        public void DeleteDependency(int dependentOnId, int dependentId)
        {
            var depdendency = _dependencyRepository.Get(d => d.DependentOnGroupingId == dependentOnId && d.DependentGroupingId == dependentId);
            _dependencyRepository.Delete(depdendency);
            _dependencyRepository.Save();
        }

        public Grouping CreateDependency(Dependency dependency)
        {
            var domainDepdendency = Mapper.Map<DataAccess.Models.Dependency>(dependency);
            _dependencyRepository.Add(domainDepdendency);
            _dependencyRepository.Save();
            return Mapper.Map<Grouping>(_groupingRepository.Get(g => g.GroupingId == domainDepdendency.DependentOnGroupingId));
        
        
        }
    }
}
