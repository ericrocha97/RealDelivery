using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [HttpGet]
        public ActionResult Index(string att)
        {
            if(att == "valido")
            {
                var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim("Login", "admin")
            }, "ApplicationCookie");
                Request.GetOwinContext().Authentication.SignIn(identity);

            }
            if(att == "nao")
            {
                Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            }
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