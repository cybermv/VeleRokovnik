using System.Web.Mvc;

namespace VeleRokovnik.Controllers
{
    [Authorize]
    public class RokovnikController : Controller
    {
        // GET: /
        // prvi splash screen koji korisnik vidi - ne mora biti prijavljen
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Pregled");
            }

            return this.View();
        }

        // GET: Rokovnik/Pregled
        // glavna stranica sa pregledom obveza
        public ActionResult Pregled()
        {
            return View();
        }
    }
}