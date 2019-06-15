using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace RealDelivery.Controllers
{
    public class HomeController : Controller
    {
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();
        public ActionResult Index()
        {
            var s = "S";
            var dataset = db.grupo.Where(x => x.grupo_ativo == s).ToList();
            return View(dataset);
        }

        public ActionResult Produtos(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //produto produto = db.produto.;
            var dataset = db.produto.Where(x => x.grupo_cod == id && x.produto_ativo == "S").ToList();
            if (dataset == null)
            {
                return HttpNotFound();
            }
            return View(dataset);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Descrição da Pagina.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Pagina de contato.";
            return View();
        }
    }
}