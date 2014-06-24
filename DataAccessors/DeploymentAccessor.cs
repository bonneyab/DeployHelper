using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core;
using DataAccess.Models;
using DataAccess.Repository;
using Dependency = Contract.DTO.Dependency;
using Grouping = Contract.DTO.Grouping;
using Step = Contract.DTO.Step;

namespace DataAccessors
{
    public interface IDeploymentAccessor : IDependency
    {
        List<Contract.DTO.Deployment> GetDeployments();
        Contract.DTO.Deployment GetDeployment(int deploymentId);
        void CreateDeployment(Contract.DTO.Deployment deployment);
        void DeleteDeployment(int deploymentId);
        void EditDeployment(Contract.DTO.Deployment deployment);
        void ResetDeployment(int deploymentId);
    }

    //TODO: I'd like to redo this in a generic/reflective manner so that mappings are automatically created for all contract/model items and this should probably get moved.
    public class MappingHelper
    {
        public static void SetupMaps()
        {
            Mapper.CreateMap<Deployment, Contract.DTO.Deployment>();
            Mapper.CreateMap<Contract.DTO.Deployment, Deployment>();

            Mapper.CreateMap<List<DataAccess.Models.Dependency>, List<Dependency>>();
            Mapper.CreateMap<DataAccess.Models.Dependency, Dependency>();
            Mapper.CreateMap<Dependency, DataAccess.Models.Dependency>();

            Mapper.CreateMap<List<DataAccess.Models.Grouping>, List<Grouping>>();
            Mapper.CreateMap<DataAccess.Models.Grouping, Grouping>();
            Mapper.CreateMap<Grouping, DataAccess.Models.Grouping>();

            Mapper.CreateMap<List<DataAccess.Models.Step>, List<Step>>();
            Mapper.CreateMap<DataAccess.Models.Step, Step>();
            Mapper.CreateMap<Step, DataAccess.Models.Step>();
        }
    }

    public class DeploymentAccessor : IDeploymentAccessor
    {
        //TODO consider higher level location for all the automappers stuff.
        private readonly IRepository<Deployment> _deploymentRepository;
        private readonly IGroupingsAccessor _groupingsAccessor;
        private readonly IRepository<DataAccess.Models.Grouping> _groupingRepository;

        public DeploymentAccessor(IRepository<Deployment> deploymentRepository, IGroupingsAccessor groupingsAccessor, IRepository<DataAccess.Models.Grouping> groupingRepository)
        {
            _deploymentRepository = deploymentRepository;
            _groupingsAccessor = groupingsAccessor;
            _groupingRepository = groupingRepository;
        }

        public List<Contract.DTO.Deployment> GetDeployments()
        {
            var deployments = _deploymentRepository.GetAll().ToList();
            var result = Mapper.Map<List<Deployment>, List<Contract.DTO.Deployment>>(deployments);
            return result;
        }

        public Contract.DTO.Deployment GetDeployment(int deploymentId)
        {
            var deployment = _deploymentRepository.Get(s => s.DeploymentId == deploymentId);
            return Mapper.Map<Contract.DTO.Deployment>(deployment);
        }

        public void CreateDeployment(Contract.DTO.Deployment deployment)
        {
            _deploymentRepository.Add(Mapper.Map<Deployment>(deployment));
            _deploymentRepository.Save();
        }

        public void DeleteDeployment(int deploymentId)
        {
            var groupings = _groupingRepository.FindBy(g => g.DeploymentId == deploymentId).ToList();
            groupings.ForEach(g => _groupingsAccessor.DeleteGrouping(g.GroupingId));
            _deploymentRepository.DeleteById(deploymentId);
            _deploymentRepository.Save();
        }

        public void EditDeployment(Contract.DTO.Deployment deployment)
        {
            _deploymentRepository.Edit(Mapper.Map<Deployment>(deployment));
            _deploymentRepository.Save();
        }

        public void ResetDeployment(int deploymentId)
        {
            var groupings = _groupingRepository.FindBy(g => g.DeploymentId == deploymentId).ToList();
            groupings.ForEach(_groupingsAccessor.ResetGrouping);
        }
    }
}
