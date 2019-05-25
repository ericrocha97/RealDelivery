using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealDelivery;

namespace RealDelivery.Controllers
{
    public class EnderecoController : Controller
    {
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();

        [Authorize]
        // GET: Endereco
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var endereco = db.endereco.Where(e => e.cliente_cod == id);
            if (endereco == null)
            {
                return HttpNotFound();
            }
            return View(endereco.ToList());
        }


        [Authorize]
        // GET: Endereco/Create
        public ActionResult Create(Int32 id)
        {
            endereco endereco = new endereco();
            endereco.cliente_cod = id;

            return View(endereco);


        }

        // POST: Endereco/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Include = "endereco_cod,cliente_cod,endereco_cep,endereco_rua,endereco_bairro,endereco_cidade,endereco_uf,endereco_comp,endereco_num")] endereco endereco)
        {
            if (ModelState.IsValid)
            {
                db.endereco.Add(endereco);
                db.SaveChanges();
                return RedirectToAction("Panel","Cliente");
            }

            ViewBag.cliente_cod = endereco.cliente_cod;
            return View(endereco);
        }
        [Authorize]
        // GET: Endereco/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            endereco endereco = db.endereco.Find(id);
            if (endereco == null)
            {
                return HttpNotFound();
            }
            ViewBag.cliente_cod = new SelectList(db.cliente, "cliente_cod", "cliente_nome", endereco.cliente_cod);
            return View(endereco);
        }

      
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Edit([Bind(Include = "endereco_cod,cliente_cod,endereco_cep,endereco_rua,endereco_bairro,endereco_cidade,endereco_uf,endereco_comp,endereco_num")] endereco endereco)
        {
            if (ModelState.IsValid)
            {
                var id = endereco.cliente_cod;
                db.Entry(endereco).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/Endereco/Index/" + id);
                
            }
            ViewBag.cliente_cod = new SelectList(db.cliente, "cliente_cod", "cliente_nome", endereco.cliente_cod);
            return View(endereco);
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            
            endereco endereco = db.endereco.Find(id);
            var cod = endereco.cliente_cod;
            db.endereco.Remove(endereco);
            db.SaveChanges();
            return Redirect("~/Endereco/Index/" + cod);
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
