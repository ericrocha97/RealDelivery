using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
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
        public ActionResult Create([Bind(Include = "grupo_cod,grupo_nome,grupo_ativo,grupo_desc,grupo_img, img_grupo")] grupo grupo, HttpPostedFileBase file)
        {
            if (db.grupo.Count(u => u.grupo_nome == grupo.grupo_nome) > 0)
            {
                ModelState.AddModelError("grupo_nome", "Esse grupo já está cadastrado");
                return View(grupo);

            }

            var imageTypes = new string[]{
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
            };

            if (file != null)
            {
                if (!imageTypes.Contains(file.ContentType))
                {
                    ModelState.AddModelError("img_grupo", "Escolha uma imagem GIF, JPG ou PNG.");
                }
            }

            if (grupo.grupo_nome == null)
            {
                ModelState.AddModelError("grupo_nome", "Nome do grupo deve ser preenchido.");
                return View(grupo);
            }

            if (grupo.grupo_nome != null)
            {
                if (grupo.grupo_nome.Length > 50)
                {
                    ModelState.AddModelError("grupo_nome", "Nome do grupo deve ser somente até 50 caracteres.");
                    return View(grupo);
                }
            }

            if (grupo.grupo_desc.Length > 500)
            {
                ModelState.AddModelError("grupo_desc", "Descrição é muito grande.");
                return View(grupo);
            }


            //if (ModelState.IsValid)
            {
                if (grupo.grupo_ativo == null)
                {
                    grupo.grupo_ativo = "N";
                }

                if (file != null)
                {
                    var imagemNome = String.Format("{0:yyyyMMdd-HHmmssfff}", DateTime.Now);
                    var extensao = System.IO.Path.GetExtension(file.FileName).ToLower();

                    using (var img = System.Drawing.Image.FromStream(file.InputStream))
                    {
                        grupo.grupo_img = String.Format("/Imagem/Grupos/{0}{1}", imagemNome, extensao);
                        // Salva imagem 
                        SalvarNaPasta(img, grupo.grupo_img);
                    }
                }

                db.grupo.Add(grupo);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            //return View(grupo);
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
        public ActionResult Edit([Bind(Include = "grupo_cod,grupo_nome,grupo_ativo,grupo_desc,grupo_img")] grupo grupo, HttpPostedFileBase file)
        {
            if (db.grupo.Count(u => u.grupo_nome == grupo.grupo_nome && u.grupo_cod != grupo.grupo_cod) > 0)
            {
                ModelState.AddModelError("grupo_nome", "Esse grupo já está cadastrado");
                return View(grupo);

            }

            var imageTypes = new string[]{
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
            };
            if (file != null)
            {
                if (!imageTypes.Contains(file.ContentType))
                {
                    ModelState.AddModelError("img_grupo", "Escolha uma imagem GIF, JPG ou PNG.");
                }
            }


            if (grupo.grupo_nome == null)
            {
                ModelState.AddModelError("grupo_nome", "Nome do grupo deve ser preenchido.");
                return View(grupo);
            }

            if (grupo.grupo_nome != null)
            {
                if (grupo.grupo_nome.Length > 50)
                {
                    ModelState.AddModelError("grupo_nome", "Nome do grupo deve ser somente até 50 caracteres.");
                    return View(grupo);
                }
            }

            if (grupo.grupo_desc.Length > 500)
            {
                ModelState.AddModelError("grupo_desc", "Descrição é muito grande.");
                return View(grupo);
            }

            //if (ModelState.IsValid)
            {
                if (grupo.grupo_ativo == null)
                {
                    grupo.grupo_ativo = "N";
                }

                if(file != null)
                {
                    var imagemNome = String.Format("{0:yyyyMMdd-HHmmssfff}", DateTime.Now);
                    var extensao = System.IO.Path.GetExtension(file.FileName).ToLower();

                    using (var img = System.Drawing.Image.FromStream(file.InputStream))
                    {
                        grupo.grupo_img = String.Format("/Imagem/Grupos/{0}{1}", imagemNome, extensao);
                        // Salva imagem 
                        SalvarNaPasta(img, grupo.grupo_img);
                    }

                }


                db.Entry(grupo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grupo);
        }

        private void SalvarNaPasta(Image img, string caminho)
        {
            // Obtém a nova resolução
            //Size tamanhoImagem = NovoTamanhoImagem(img.Size, novoTamanho);
            using (System.Drawing.Image novaImagem = new Bitmap(img))
            {
                novaImagem.Save(Server.MapPath(caminho), img.RawFormat);
            }
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
