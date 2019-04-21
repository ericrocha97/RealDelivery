using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealDelivery;

namespace RealDelivery.Controllers
{
    public class produtoController : Controller
    {
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();

        // GET: produto
        [Authorize]
        public ActionResult Index()
        {
            var s = "S";
            var dataset = db.produto.Where(x => x.produto_ativo == s).ToList();
            return View(dataset);
        }
        [Authorize]
        public ActionResult Buscar()
        {
            return View();
        }

        //GET
        [HttpGet]
        [Authorize]
        public ActionResult Buscar(string texto, string ativo)
        {
            if (ativo == null)
            {
                ativo = "N";
            }
            var dataset = db.produto.Where(x => (x.produto_nome.Contains(texto) || x.grupo.grupo_nome.Contains(texto)) && x.produto_ativo == ativo).ToList();
            return View(dataset);

        }

        // GET: produto/Details/5
        [Authorize]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto produto = db.produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: produto/Create
        [Authorize]
        public ActionResult Create()
        {
            //ViewBag.grupo_cod = new SelectList(db.grupo, "grupo_cod", "grupo_nome");
            var s = "S";
            ViewBag.grupo_cod = new SelectList(db.grupo.Where(x => x.grupo_ativo == s), "grupo_cod", "grupo_nome");
            ViewBag.impress_cod = new SelectList(db.impress, "impress_cod", "impress_cod");
            return View();
        }

        // POST: produto/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Include = "produto_cod,produto_nome,grupo_cod,produto_preco,produto_custo,produto_ativo,impress_cod,produto_desc,produto_img, img_produto")] produto produto)
        {
            
            if (ModelState.IsValid)
            {
                if (produto.produto_ativo == null)
                {
                    produto.produto_ativo = "N";
                }

                if (produto.img_produto != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(produto.img_produto.FileName);
                    string extension = Path.GetExtension(produto.img_produto.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    produto.produto_img = "~/Imagem/Produto/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Imagem/Grupos/"), fileName);
                    produto.img_produto.SaveAs(fileName);
                }
                


                db.produto.Add(produto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var s = "S";
            ViewBag.grupo_cod = new SelectList(db.grupo.Where(x => x.grupo_ativo == s), "grupo_cod", "grupo_nome", produto.grupo_cod);
            ViewBag.impress_cod = new SelectList(db.impress, "impress_cod", "impress_cod", produto.impress_cod);
            return View(produto);
        }

        // GET: produto/Edit/5
        [Authorize]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto produto = db.produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            var s = "S";
            ViewBag.grupo_cod = new SelectList(db.grupo.Where(x => x.grupo_ativo == s), "grupo_cod", "grupo_nome", produto.grupo_cod);
            ViewBag.impress_cod = new SelectList(db.impress, "impress_cod", "impress_cod", produto.impress_cod);
            return View(produto);
        }

        // POST: produto/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Edit([Bind(Include = "produto_cod,produto_nome,grupo_cod,produto_preco,produto_custo,produto_ativo,impress_cod,produto_desc,produto_img, img_produto")] produto produto)
        {
            if (ModelState.IsValid)
            {
                if (produto.produto_ativo == null)
                {
                    produto.produto_ativo = "N";
                }
                
                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var s = "S";
            ViewBag.grupo_cod = new SelectList(db.grupo.Where(x => x.grupo_ativo == s), "grupo_cod", "grupo_nome", produto.grupo_cod);
            ViewBag.impress_cod = new SelectList(db.impress, "impress_cod", "impress_cod", produto.impress_cod);
            return View(produto);
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
