using System.Collections.Generic;
using AutoMapper;
using Contract.DTO;
using Core;
using DataAccess.Repository;

namespace DataAccessors
{
    public interface IStepAccessor : IDependency
    {
        List<Step> GetStepsByGrouping(int groupingId);
        Step CreateStep(Step step);
        void UpdateStep(Step step);
        void DeleteStep(int stepId);
        void CompleteStep(int stepId, bool complete);
        Step GetStep(int stepId);
    }

    public class StepAccessor : IStepAccessor
    {
        private readonly IRepository<DataAccess.Models.Step> _stepRepository;

        public StepAccessor(IRepository<DataAccess.Models.Step> stepRepository)
        {
            _stepRepository = stepRepository;
        }

        public void DeleteStep(int stepId)
        {
            _stepRepository.DeleteById(stepId);
            _stepRepository.Save();
        }

        //TODO: I don't really like this name but nothing better is coming immediately to mind. Further this sort of pattern is just more reason to cache the whole thing...
        //TODO: A solution with redis and observable lists might be interesting?
        public void CompleteStep(int stepId, bool complete)
        {
            var step = _stepRepository.Get(s => s.StepId == stepId);
            step.Complete = complete;
            _stepRepository.Edit(step);
            _stepRepository.Save();
        }

        public Step GetStep(int stepId)
        {
            return Mapper.Map<Step>(_stepRepository.Get(s => s.StepId == stepId));
        }

        public List<Step> GetStepsByGrouping(int groupingId)
        {
            var steps = _stepRepository.FindBy(s => s.GroupingId == groupingId);
            return Mapper.Map<List<Step>>(steps);
        }

        public Step CreateStep(Step step)
        {
            var domainStep = Mapper.Map<DataAccess.Models.Step>(step);
            _stepRepository.Add(domainStep);
            _stepRepository.Save();
            return Mapper.Map<Step>(domainStep);
        }

        public void UpdateStep(Step step)
        {
            _stepRepository.Edit(Mapper.Map<DataAccess.Models.Step>(step));
            _stepRepository.Save();
        }
    }
}
