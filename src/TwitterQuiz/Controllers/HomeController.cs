using System.Web.Mvc;
using SimpleAuthentication.Mvc;

namespace TwitterQuiz.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {

            return View();
        }

    }
}
