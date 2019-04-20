using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using RealDelivery.ViewModels;

namespace RealDelivery.Controllers
{
    public class AdministradorController : Controller
    {
        // GET: Administrador
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();

        
        [HttpGet]
        public ActionResult Login(LoginViewModel viewmodel)
        {
            if (!ModelState.IsValid)

            {
                return View(viewmodel);
            }

            var usuario = db.usuario.FirstOrDefault(u => u.usuario_email == viewmodel.email);

            if (usuario == null)
            {
                ModelState.AddModelError("Login", "Login incorreto");
                return View(viewmodel);
            }

            if (usuario.usuario_senha != viewmodel.Password)
            {
                ModelState.AddModelError("Senha", "Senha incorreta");
                return View(viewmodel);
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.usuario_nome),
                new Claim("Login", usuario.usuario_email)
            }, "ApplicationCookie");

            Request.GetOwinContext().Authentication.SignIn(identity);

            if (!String.IsNullOrWhiteSpace(viewmodel.UrlRetorno) || Url.IsLocalUrl(viewmodel.UrlRetorno))
                return Redirect(viewmodel.UrlRetorno);
            else
                return RedirectToAction("Index", "Home");





        }
    }
}