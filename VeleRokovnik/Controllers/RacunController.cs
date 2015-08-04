using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VeleRokovnik.DAL;
using VeleRokovnik.Entities;
using VeleRokovnik.Models;

namespace VeleRokovnik.Controllers
{
    [Authorize]
    public class RacunController : Controller
    {
        public RokovnikUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<RokovnikUserManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public IRokovnikRepository Repository { get; set; }

        public RacunController()
        {
            Repository = new RokovnikRepository(new RokovnikContext());
        }

        // GET: /Racun
        // redirecta na Kreiranje
        public ActionResult Index()
        {
            return RedirectToAction("Kreiranje");
        }

        // GET: /Racun/Prijava
        // prijava prvi screen
        [AllowAnonymous]
        public ActionResult Prijava(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Racun/Prijava
        // submittanje podataka s logina
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Prijava(PrijavaModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                RokovnikUser user = UserManager.FindByName(model.Username);
                if (user == null)
                {
                    ModelState.AddModelError("",
                        "Nismo pronašli tvoje korisničko ime u sustavu. Za kreiranje računa klikni <a href='" +
                        Url.Action("Kreiranje") + "'>ovdje</a>!");
                    return View(model);
                }

                if (!UserManager.CheckPassword(user, model.Password))
                {
                    ModelState.AddModelError("", "Kriva lozinka! Pokušaj ponovno.");
                    return View(model);
                }

                Osoba osoba = Repository.GetOsobaByKey(user.OsobaId);
                ClaimsIdentity identity = CreateIdentityWithClaims(user, osoba);

                AuthenticationManager.SignIn(new AuthenticationProperties(), identity);

                return RedirectToLocal(returnUrl);
            }

            return View(model);
        }

        // GET: /Racun/Kreiranje
        // registracija korisnika
        [AllowAnonymous]
        public ActionResult Kreiranje()
        {
            return View();
        }

        // POST: /Racun/Kreiranje
        // submittanje podataka za novi acc
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Kreiranje(NoviKorisnikModel model)
        {
            if (ModelState.IsValid)
            {
                RokovnikUser newUser = new RokovnikUser
                {
                    UserName = model.Username,
                    OsobaId = -1
                };

                var result = UserManager.Create(newUser, model.Password);

                if (result.Succeeded)
                {
                    Osoba newOsoba = new Osoba
                    {
                        Ime = model.Ime,
                        Prezime = model.Prezime,
                        NazivRokovnika = "Novi rokovnik",
                        OpisRokovnika = "Prazni opis rokovnika",
                        DatumRodjenja = model.DatumRodjenja
                    };

                    newUser.OsobaId = Repository.AddOsoba(newOsoba);
                    UserManager.Update(newUser);

                    ClaimsIdentity identity = CreateIdentityWithClaims(newUser, newOsoba);
                    AuthenticationManager.SignIn(new AuthenticationProperties(), identity);

                    return RedirectToAction("Index", "Rokovnik");
                }
                else
                {
                    ModelState.AddModelError("type", "danger");
                    ModelState.AddModelError("", "Korisničko ime je zauzeto! Pokušaj s drugim imenom.");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        // GET: /Racun/Podaci
        // pregled i izmjena osobnih podataka
        public ActionResult Podaci()
        {
            var osoba = Repository.GetOsobaByKey(User.OsobaId());
            var model = new OsobniPodaciModel(osoba);
            return View(model);
        }

        // POST: /Racun/Podaci
        // submittanje novih podataka
        [HttpPost]
        public ActionResult Podaci(OsobniPodaciModel model)
        {
            if (ModelState.IsValid)
            {
                Osoba osobaToEdit = Repository.GetOsobaByKey(User.OsobaId());

                model.UpdateOsoba(ref osobaToEdit);
                Repository.UpdateOsoba(osobaToEdit);
                if (Repository.SaveChanges())
                {
                    ModelState.AddModelError("type", "success");
                    ModelState.AddModelError("", "Uspješno spremljene promjene!");

                    var userId = User.Identity.GetUserId();
                    SignOutWhoeverIsLoggedIn();
                    var user = UserManager.FindById(userId);
                    ClaimsIdentity toLogin = CreateIdentityWithClaims(user, osobaToEdit);
                    AuthenticationManager.SignIn(new AuthenticationProperties(), toLogin);

                    return View(model);
                }
            }

            return View(model);
        }

        // GET: /Racun/PromjenaLozinke
        // unos nove lozinke
        [HttpGet]
        public ActionResult PromjenaLozinke()
        {
            return View();
        }

        // POST: /Racun/PromjenaLozinke
        // submittanje nove lozinke
        [HttpPost]
        public ActionResult PromjenaLozinke(PromjenaLozinkeModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = UserManager.Find(User.Identity.GetUserName(), model.StaraLozinka);
                if (currentUser != null)
                {
                    IdentityResult response = UserManager.ChangePassword(currentUser.Id, model.StaraLozinka, model.NovaLozinka);
                    if (response.Succeeded)
                    {
                        SignOutWhoeverIsLoggedIn();
                        return RedirectToAction("Prijava");
                    }
                }
                ModelState.AddModelError("", "Nema takvog korisnika u bazi!");
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        // GET: /Racun/ResetLozinke
        // unos nove lozinke (reset)
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetLozinke()
        {
            return View();
        }

        // POST: /Racun/ResetLozinke
        // submittanje nove lozinke (reset)
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetLozinke(ResetLozinkeModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = UserManager.FindByName(model.Username);
                if (currentUser != null)
                {
                    UserManager.RemovePassword(currentUser.Id);
                    IdentityResult response = UserManager.AddPassword(currentUser.Id, model.NovaLozinka);

                    if (response.Succeeded)
                    {
                        SignOutWhoeverIsLoggedIn();
                        return RedirectToAction("Prijava");
                    }
                }
                ModelState.AddModelError("", "Nema takvog korisnika u bazi!");
                return View(model);
            }
            else
            {
                return View(model);
            }

        }

        // GET: /Racun/Odjava
        // odjava iz sustava
        public ActionResult Odjava()
        {
            SignOutWhoeverIsLoggedIn();
            return RedirectToAction("Index", "Rokovnik");
        }

        #region Private functions
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Rokovnik");
        }

        private ClaimsIdentity CreateIdentityWithClaims(RokovnikUser user, Osoba osoba)
        {
            ClaimsIdentity identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim("OsobaId", user.OsobaId.ToString("D")));
            identity.AddClaim(new Claim("OsobaIme", osoba.Ime));
            identity.AddClaim(new Claim("OsobaPrezime", osoba.Prezime));
            identity.AddClaim(new Claim("OsobaNazivRokovnika", osoba.NazivRokovnika));
            identity.AddClaim(new Claim("OsobaOpisRokovnika", osoba.OpisRokovnika));

            return identity;
        }

        private void SignOutWhoeverIsLoggedIn()
        {
            AuthenticationManager.SignOut(new AuthenticationProperties());
        }
        #endregion
    }
}