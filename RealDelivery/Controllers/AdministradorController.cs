using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealDelivery.Controllers
{
    public class AdministradorController : Controller
    {
        // GET: Administrador
        public ActionResult Login()
        {
            return View();
        }
    }
}