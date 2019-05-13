using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using RealDelivery.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace RealDelivery.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Administrador
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginClienteViewModel viewmodel)
        {
            if (!ModelState.IsValid)

            {
                return View(viewmodel);
            }

            var cliente = db.cliente.FirstOrDefault(u => u.cliente_email == viewmodel.cliente_email);
            var SenhaMD5 = viewmodel.password;

            if (cliente == null)
            {
                ModelState.AddModelError("Login", "Login incorreto");
                return View(viewmodel);
            }

            if (cliente.cliente_senha != SenhaMD5)
            {
                ModelState.AddModelError("Senha", "Senha incorreta");
                return View(viewmodel);
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.cliente_nome),
                new Claim("Login", cliente.cliente_email)
            }, "ApplicationCookie");

            Request.GetOwinContext().Authentication.SignIn(identity);

            if (!String.IsNullOrWhiteSpace(viewmodel.UrlRetorno) || Url.IsLocalUrl(viewmodel.UrlRetorno))
                return Redirect(viewmodel.UrlRetorno);
            else
                return RedirectToAction("Index", "Home");

        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }







    }
}
