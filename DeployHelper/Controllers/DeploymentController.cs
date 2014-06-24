using System.Web.Mvc;
using Contract.DTO;
using DataAccessors;
using DeploymentHelperServices.ViewModelProviders;

namespace DeployHelper.Controllers
{
    //TODO: Maybe think about redoing this following all the async/await business.
    public class DeploymentController : Controller
    {
        private readonly IDeploymentAccessor _deploymentAccessor;
        private readonly IDeploymentDetailsViewModelProvider _deploymentDetailsViewModelProvider;

        public DeploymentController(IDeploymentAccessor deploymentAccessor,
            IDeploymentDetailsViewModelProvider deploymentDetailsViewModelProvider)
        {
            _deploymentAccessor = deploymentAccessor;
            _deploymentDetailsViewModelProvider = deploymentDetailsViewModelProvider;
        }

        public ActionResult Index()
        {
            return View(_deploymentAccessor.GetDeployments());
        }

        public ActionResult Details(int deploymentId)
        {
            return View(_deploymentDetailsViewModelProvider.GetDeploymentDetailsViewModel(deploymentId));
        }


        //TODO: Should this route through create? seems like it could cause trouble. Maybe just pull some of it into a partial or something later to avoid the duplication.
        public ActionResult Edit(int deploymentId)
        {
            var model = _deploymentAccessor.GetDeployment(deploymentId);
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Deployment deployment)
        {
            _deploymentAccessor.EditDeployment(deployment);
            return RedirectToAction("Details", new { deploymentId = deployment.DeploymentId });
        }

        public ActionResult Create()
        {
            return View();
        }

        //TODO: Totally bad practice having a delete that isn't a post, easy for now...
        public ActionResult Delete(int deploymentId)
        {
            _deploymentAccessor.DeleteDeployment(deploymentId);
            return View("Index", _deploymentAccessor.GetDeployments());
        }

        //TODO: make this a post eventually
        public ActionResult Reset(int deploymentId)
        {
            _deploymentAccessor.ResetDeployment(deploymentId);
            return RedirectToAction("Details", new { deploymentId });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Contract.DTO.Deployment deployment)
        {
            _deploymentAccessor.CreateDeployment(deployment);
            return RedirectToAction("Index");
        }
    }
}