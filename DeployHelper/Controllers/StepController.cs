using System.Web.Mvc;
using Contract.DTO;
using DataAccessors;

namespace DeployHelper.Controllers
{
    public class StepController : Controller
    {
        private readonly IStepAccessor _stepAccessor;

        public StepController(IStepAccessor stepAccessor)
        {
            _stepAccessor = stepAccessor;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Create(Step step)
        {
            var newStep = _stepAccessor.CreateStep(step);
            return Json(newStep);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Delete(int stepId)
        {
            _stepAccessor.DeleteStep(stepId);
            return Json(true);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Complete(int stepId, bool complete)
        {
            _stepAccessor.CompleteStep(stepId, complete);
            return Json(true);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Edit(Step step)
        {
            _stepAccessor.UpdateStep(step);
            return Json(true);
        }

        public JsonResult Get(int stepId)
        {
            var step = _stepAccessor.GetStep(stepId);
            return Json(step, JsonRequestBehavior.AllowGet);
        }
    }
}