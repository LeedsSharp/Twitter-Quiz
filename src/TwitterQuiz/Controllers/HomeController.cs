using System.Web.Mvc;

namespace TwitterQuiz.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            ViewBag.Username = User.Identity.Name;
            return View();
        }

    }
}
