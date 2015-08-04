using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using VeleRokovnik.Entities;
using VeleRokovnik.Models;
using VeleRokovnik.DAL;
using VeleRokovnik.Util;

namespace VeleRokovnik.Controllers
{
    [Authorize]
    public class ObvezeController : Controller
    {
        public ObvezeController()
        {
            this.Repository = new RokovnikRepository(new RokovnikContext());
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return Json(data, contentType, contentEncoding, JsonRequestBehavior.DenyGet);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        private RokovnikRepository Repository { get; set; }

        // GET: /Obveze/Get?osobaId=5
        // dohvat liste obveza za korisnika
        public ActionResult Get(int osobaId)
        {
            IList<Obveza> model = Repository.GetObvezeFor(osobaId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GET: /Obveze/Get?obvezaId=8
        // dohvat pojedine obveze
        public ActionResult GetById(int obvezaId)
        {
            Obveza foundObveza = Repository.GetObvezaByKey(obvezaId);
            return Json(foundObveza, JsonRequestBehavior.AllowGet);
        }

        // GET: /Obveze/New
        // kreira novu obvezu
        public ActionResult New()
        {
            Obveza newObveza = new Obveza();
            return Json(newObveza, JsonRequestBehavior.AllowGet);
        }

        // POST: /Obveze/Save
        // unos nove obveze
        [HttpPost]
        public ActionResult Save(Obveza model)
        {
            if (!model.OsobaId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (model.ObvezaId.HasValue)
                Repository.UpdateObveza(model);
            else
                model.ObvezaId = Repository.AddObvezaFor(model.OsobaId.Value, model);

            Repository.SaveChanges();
            return Json(model);
        }

        // POST: /Obveze/Obrisi?obvezaId=8
        // brise obvezu
        [HttpPost]
        public ActionResult Delete(int obvezaId)
        {
            Repository.DeleteObveza(obvezaId);
            Repository.SaveChanges();
            return Json(obvezaId);
        }

        // GET: /Obveze/GetVrsteObveze
        // vraća niz key-value parova za dropdown
        public ActionResult GetVrsteObveze()
        {
            return Json(Obveza.GetVrstaObvezeSelectList(), JsonRequestBehavior.AllowGet);
        }
    }
}