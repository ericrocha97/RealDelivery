using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using RealDelivery.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity;
using System.Net;

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
            var SenhaMD5 = GerarHashMd5(viewmodel.password);
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
                new Claim(ClaimTypes.Name, cliente.cliente_email),
                new Claim(ClaimTypes.Role, "Cliente".ToString())
            }, "ApplicationCookie");
            Request.GetOwinContext().Authentication.SignIn(identity);
            if (!String.IsNullOrWhiteSpace(viewmodel.UrlRetorno) || Url.IsLocalUrl(viewmodel.UrlRetorno))
                return Redirect(viewmodel.UrlRetorno);
            else
                return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home"); //ApplicationCookie
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: ex/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "cliente_cod,cliente_nome,cliente_telefone,cliente_email,cliente_senha")] cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var senha = GerarHashMd5(cliente.cliente_senha);
                cliente.cliente_senha = senha;
                db.cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(cliente);
        }

        [Authorize(Roles = "Cliente")]
        // GET: ex/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cliente cliente = db.cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult Panel()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var cliente = db.cliente.FirstOrDefault(x => x.cliente_email == userName);

                if (cliente != null)
                    return View(cliente);
            }
            return View(db.cliente.ToList());
        }

        // POST: ex/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public ActionResult Edit([Bind(Include = "cliente_cod,cliente_nome,cliente_telefone,cliente_email,cliente_senha")] cliente cliente, string SenhaNova)
        {
            if (ModelState.IsValid)
            {
                if (SenhaNova != "")
                {
                    cliente.cliente_senha = GerarHashMd5(SenhaNova);
                }
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Panel");
            }
            return View(cliente);
        }
        [Authorize(Roles = "Cliente")]
        public ActionResult Pedidos(int? id)
        {

            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var pedido = db.pedido.Include(p => p.cliente).Include(p => p.endereco).Where(x => x.cliente_cod == id).OrderByDescending(x => x.pedido_data);
                return View(pedido.ToList());
            }

        }
        [Authorize(Roles = "Cliente")]
        public ActionResult DetalhePedido(int? id)
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
    }
}
