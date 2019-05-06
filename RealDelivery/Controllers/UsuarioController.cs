using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RealDelivery;

namespace RealDelivery.Controllers
{
    public class UsuarioController : Controller
    {
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();

        // GET: Usuario
        [Authorize]
        public ActionResult Index()
        {
            return View(db.usuario.ToList());
        }


        // GET: Usuario/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
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
        // POST: Usuario/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Include = "usuario_cod,usuario_email,usuario_senha,usuario_nome,usuario_cel")] usuario usuario)
        {
            if (db.usuario.Count(u => u.usuario_email == usuario.usuario_email) > 0)
                {
                ModelState.AddModelError("usuario_email", "Esse e-mail já está cadastrado");
                return View(usuario);
            }

            if (ModelState.IsValid)
            {
                var SenhaMD5 = GerarHashMd5(usuario.usuario_senha);
                usuario.usuario_senha = SenhaMD5;
                db.usuario.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }
        [Authorize]
        public ActionResult Buscar()
        {
            return View();
        }

        //GET
        [HttpGet]
        [Authorize]
        public ActionResult Buscar(string texto)
        {

            var dataset = db.usuario.Where(x => (x.usuario_nome.Contains(texto) || x.usuario_email.Contains(texto) || x.usuario_cel.Contains(texto))).ToList();
            return View(dataset);

        }

        // GET: Usuario/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Edit([Bind(Include = "usuario_cod,usuario_email,usuario_senha,usuario_nome,usuario_cel")] usuario usuario)
        {
            if (ModelState.IsValid)
            {


                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }
        [Authorize]
        public ActionResult EditPassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
        [HttpPost]
        [Authorize]
        public ActionResult EditPassword([Bind(Include = "usuario_cod,usuario_email,usuario_senha,usuario_nome,usuario_cel")] usuario usuario, string senha_nova, string senha_nova_confirm)
        {
            if (ModelState.IsValid)
            {
                var senha = GerarHashMd5(usuario.usuario_senha);
                var cod = usuario.usuario_cod;
                var result = db.usuario.Count(x => x.usuario_senha.Equals(senha) && x.usuario_cod.Equals(cod));
                if (result == 1)
                {
                    if(senha_nova == senha_nova_confirm)
                    {
                        usuario.usuario_senha = GerarHashMd5(senha_nova);
                    }
                }

                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
