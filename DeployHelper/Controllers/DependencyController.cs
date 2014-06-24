using System.Web.Mvc;
using Contract.DTO;
using DataAccessors;

namespace DeployHelper.Controllers
{
    public class DependencyController : Controller
    {
        private readonly IDependencyAccessor _dependencyAccessor;
        private readonly IGroupingsAccessor _groupingsAccessor;

        public DependencyController(IDependencyAccessor dependencyAccessor, IGroupingsAccessor groupingsAccessor)
        {
            _dependencyAccessor = dependencyAccessor;
            _groupingsAccessor = groupingsAccessor;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Create(Dependency dependency)
        {
            var newStep = _dependencyAccessor.CreateDependency(dependency);
            return Json(newStep);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Delete(int dependentOnId, int dependentId)
        {
            _dependencyAccessor.DeleteDependency(dependentOnId, dependentId);
            var dependent = _groupingsAccessor.GetGrouping(dependentOnId);
            return Json(dependent);
        }
    }
}