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
    public class GrupoController : Controller
    {
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();
        // GET: Grupo
        [Authorize]
        public ActionResult Index()
        {
            var s = "S";
            var dataset = db.grupo.Where(x => x.grupo_ativo == s).ToList();
            return View(dataset);


        }
        [Authorize]
        public ActionResult Buscar()
        {
            return View();
        }

        //GET
        [Authorize]
        [HttpGet]
        public ActionResult Buscar(string texto, string ativo)
        {
            if (ativo == null)
            {
                ativo = "N";
            }
            var dataset = db.grupo.Where(x => x.grupo_nome.Contains(texto) && x.grupo_ativo == ativo).ToList();
            return View(dataset);

        }
        // GET: Grupo/Details/5
        [Authorize]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupo grupo = db.grupo.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // GET: Grupo/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Grupo/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Include = "grupo_cod,grupo_nome,grupo_ativo,grupo_desc,grupo_img, img_grupo")] grupo grupo)
        {
            if (ModelState.IsValid)
            {
                if(grupo.grupo_ativo == null)
                {
                    grupo.grupo_ativo = "N";
                }
                
                if(grupo.img_grupo != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(grupo.img_grupo.FileName);
                    string extension = Path.GetExtension(grupo.img_grupo.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    grupo.grupo_img = "~/Imagem/Grupos/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Imagem/Grupos/"), fileName);
                    grupo.img_grupo.SaveAs(fileName);
                }
                

                db.grupo.Add(grupo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grupo);
        }


        // GET: Grupo/Edit/5
        [Authorize]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            grupo grupo = db.grupo.Find(id);

            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // POST: Grupo/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public ActionResult Edit([Bind(Include = "grupo_cod,grupo_nome,grupo_ativo,grupo_desc,grupo_img")] grupo grupo)
        {
            if (ModelState.IsValid)
            {
                if (grupo.grupo_ativo == null)
                {
                    grupo.grupo_ativo = "N";
                }
                db.Entry(grupo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grupo);
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
