using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealDelivery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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