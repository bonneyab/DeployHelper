using System.Web.Mvc;
using DataAccessors;
using DeploymentHelperServices.ViewModelProviders;

namespace DeployHelper.Controllers
{
    //TODO: my naming is all over the place on this thing...
    public class GroupingsController : Controller
    {
        private readonly IGroupingsAccessor _groupingsAccessor;
        private readonly IGroupDetailsViewModelProvider _groupDetailsViewModelProvider;

        public GroupingsController(IGroupingsAccessor groupingsAccessor, IGroupDetailsViewModelProvider groupDetailsViewModelProvider)
        {
            _groupingsAccessor = groupingsAccessor;
            _groupDetailsViewModelProvider = groupDetailsViewModelProvider;
        }

        public ActionResult Details(int groupingId)
        {
            return View(_groupDetailsViewModelProvider.GetGroupDetailsViewModel(groupingId));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Details(Contract.DTO.Grouping grouping)
        {
            _groupingsAccessor.UpdateGrouping(grouping);
            return RedirectToAction("Details", "Deployment", new { grouping.DeploymentId });
        }

        public ActionResult Edit(int groupingId)
        {
            return View(_groupingsAccessor.GetGrouping(groupingId));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Contract.DTO.Grouping grouping)
        {
            _groupingsAccessor.UpdateGrouping(grouping);
            return RedirectToAction("Details", "Groupings", new { grouping.GroupingId });
        }

        public ActionResult Create(int deploymentId)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Contract.DTO.Grouping grouping)
        {
            var createdGrouping = _groupingsAccessor.CreateGrouping(grouping);
            return RedirectToAction("Details", new { createdGrouping.GroupingId });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Delete(int groupingId)
        {
            _groupingsAccessor.DeleteGrouping(groupingId);
            return Json(true);
        }
    }
}