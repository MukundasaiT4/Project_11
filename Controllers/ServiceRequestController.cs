using System.Web.Mvc;
using Project_11.DAL;
using Project_11.Models;

namespace Project_11.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly ServiceRequestDAL _dal = new ServiceRequestDAL();

        public ActionResult Index(string category)
        {
            ViewBag.SelectedCategory = category;
            var requests = _dal.GetAllRequests(category);
            return View(requests);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceRequest model)
        {
            if (ModelState.IsValid)
            {
                _dal.AddRequest(model);
                TempData["SuccessMessage"] = "Service request submitted successfully!";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult UpdateStatus(int id)
        {
            var req = _dal.GetRequestById(id);
            if (req == null) return HttpNotFound();
            return View(req);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int RequestId, string Status)
        {
            _dal.UpdateStatus(RequestId, Status);
            TempData["SuccessMessage"] = $"Request #{RequestId} status updated to {Status}.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _dal.DeleteRequest(id);
            TempData["SuccessMessage"] = $"Request #{id} deleted.";
            return RedirectToAction("Index");
        }
    }
}