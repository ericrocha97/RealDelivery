using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using RealDelivery.ViewModels;
using System.Security.Cryptography;
using System.Text;
using static RealDelivery.CustomAuthorizeAttributed;
using System.Net;

namespace RealDelivery.Controllers
{
    public class AdministradorController : Controller
    {
        // GET: Administrador
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel viewmodel)
        {
            if (!ModelState.IsValid)

            {
                return View(viewmodel);
            }
            var usuario = db.usuario.FirstOrDefault(u => u.usuario_email == viewmodel.email);
            var SenhaMD5 = GerarHashMd5(viewmodel.password);
            if (usuario == null)
            {
                ModelState.AddModelError("Login", "Login incorreto");
                return View(viewmodel);
            }
            if (usuario.usuario_senha != SenhaMD5)
            {
                ModelState.AddModelError("Senha", "Senha incorreta");
                return View(viewmodel);
            }
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.usuario_nome),
                new Claim("Login", usuario.usuario_email),
                new Claim(ClaimTypes.Role, "Administrador")
            }, "ApplicationCookie");
            Request.GetOwinContext().Authentication.SignIn(identity);
            if (!String.IsNullOrWhiteSpace(viewmodel.UrlRetorno) || Url.IsLocalUrl(viewmodel.UrlRetorno))
                return Redirect(viewmodel.UrlRetorno);
            else
                return RedirectToAction("Panel", "Administrador");
        }

        [CustomAuthorizeAttribute(Roles = "Administrador")]
        public ActionResult Panel()
        {
            return View();
        }

        [CustomAuthorizeAttribute(Roles = "Administrador")]
        public ActionResult Pedidos()
        {

            var pedido = db.pedido.OrderByDescending(x => x.pedido_data);
            return View(pedido.ToList());


        }

        [CustomAuthorizeAttribute(Roles = "Administrador")]
        public ActionResult DetalhePedido(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pedido pedido = db.pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);

        }

        [CustomAuthorizeAttribute(Roles = "Administrador")]
        public ActionResult ItensPedido(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var item_pedido = db.item_pedido.Where(x => x.pedido_cod == id);
                return View(item_pedido.ToList());
            }

        }
        [CustomAuthorizeAttribute(Roles = "Administrador")]
        public ActionResult BuscarPedidos()
        {
            return View();
        }

        //GET
        [HttpGet]
        [CustomAuthorizeAttribute(Roles = "Administrador")]
        public ActionResult BuscarPedidos(string texto)
        {
            

            var dataset = db.pedido.Where(x => (x.pedido_cod.ToString().Contains(texto) || x.pedido_data.ToString().Contains(texto) )).ToList();
            return View(dataset);
        }

        public static string GerarHashMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Converter a String para array de bytes, que é como a biblioteca trabalha.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Cria-se um StringBuilder para recompôr a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop para formatar cada byte como uma String em hexadecimal
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }
    }
}