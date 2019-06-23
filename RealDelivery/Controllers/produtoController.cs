using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealDelivery;
using static RealDelivery.CustomAuthorizeAttributed;


namespace RealDelivery.Controllers
{
    public class produtoController : Controller
    {
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();

        // GET: produto
        [CustomAuthorizeAttribute(Roles = "Administrador")]
        public ActionResult Index()
        {
            var s = "S";
            var dataset = db.produto.Where(x => x.produto_ativo == s).ToList();
            return View(dataset);
        }
        [CustomAuthorizeAttribute(Roles = "Administrador")]
        public ActionResult Buscar()
        {
            return View();
        }

        //GET
        [HttpGet]
        [CustomAuthorizeAttribute(Roles = "Administrador")]
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
        [CustomAuthorizeAttribute(Roles = "Administrador")]
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
        [CustomAuthorizeAttribute(Roles = "Administrador")]
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
        [CustomAuthorizeAttribute(Roles = "Administrador")]
        public ActionResult Create([Bind(Include = "produto_cod,produto_nome,grupo_cod,produto_preco,produto_custo,produto_ativo,impress_cod,produto_desc,produto_img, img_produto")] produto produto, HttpPostedFile img_produto)
        {
            if (db.produto.Count(u => u.produto_nome == produto.produto_nome) > 0)
            {
                ModelState.AddModelError("produto_nome", "Esse produto já está cadastrado");
                return View(produto);
            }
            var imageTypes = new string[]{
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
            };
            if (produto.img_produto != null)
            {
                if (!imageTypes.Contains(produto.img_produto.ContentType))
                {
                    ModelState.AddModelError("img_produto", "Escolha uma imagem GIF, JPG ou PNG.");
                }
            }
            if (produto.produto_nome == null)
            {
                ModelState.AddModelError("produto_nome", "Nome do produto deve ser preenchido.");
                return View(produto);
            }
            if (produto.produto_nome != null)
            {
                if (produto.produto_nome.Length > 50)
                {
                    ModelState.AddModelError("produto_nome", "Nome do produto deve ser somente até 50 caracteres.");
                    return View(produto);
                }
            }
            if (produto.produto_preco == null)
            {
                ModelState.AddModelError("produto_preco", "Preco do produto deve ser preenchido.");
                return View(produto);
            }
            if (produto.produto_desc != null)
            {
                if (produto.produto_desc.Length > 500)
                {
                    ModelState.AddModelError("produto_desc", "Descrição é muito grande.");
                    return View(produto);
                }
            }
            if (ModelState.IsValid)
            {
                if (produto.produto_ativo == null)
                {
                    produto.produto_ativo = "N";
                }
                if (produto.img_produto != null)
                {
                    var imagemNome = String.Format("{0:yyyyMMdd-HHmmssfff}", DateTime.Now);
                    var extensao = System.IO.Path.GetExtension(produto.img_produto.FileName).ToLower();
                    using (var img = System.Drawing.Image.FromStream(produto.img_produto.InputStream))
                    {
                        produto.produto_img = String.Format("/Imagem/Produto/{0}{1}", imagemNome, extensao);
                        var produto_img_img = String.Format("~/Imagem/Produto/{0}{1}", imagemNome, extensao);

                        // Salva imagem 
                        SalvarNaPasta(img, produto_img_img);
                    }
                }
                db.produto.Add(produto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }else
            {
                if (produto.produto_ativo == null)
                {
                    produto.produto_ativo = "N";
                }
                if (produto.img_produto != null)
                {
                    var imagemNome = String.Format("{0:yyyyMMdd-HHmmssfff}", DateTime.Now);
                    var extensao = System.IO.Path.GetExtension(produto.img_produto.FileName).ToLower();
                    using (var img = System.Drawing.Image.FromStream(produto.img_produto.InputStream))
                    {
                        produto.produto_img = String.Format("~/Imagem/Produto/{0}{1}", imagemNome, extensao);
                        var produto_img_img = String.Format("~/Imagem/Produto/{0}{1}", imagemNome, extensao);
                        // Salva imagem 
                        SalvarNaPasta(img, produto_img_img);
                    }
                }
                db.produto.Add(produto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: produto/Edit/5
        [CustomAuthorizeAttribute(Roles = "Administrador")]
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
        [CustomAuthorizeAttribute(Roles = "Administrador")]
        public ActionResult Edit([Bind(Include = "produto_cod,produto_nome,grupo_cod,produto_preco,produto_custo,produto_ativo,impress_cod,produto_desc,produto_img, img_produto")] produto produto)
        {
            if (db.produto.Count(u => u.produto_nome == produto.produto_nome && u.produto_cod != produto.produto_cod) > 0)
            {
                ModelState.AddModelError("grupo_nome", "Esse grupo já está cadastrado");
                return View(produto);
            }
            var imageTypes = new string[]{
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
            };
            if (produto.img_produto != null)
            {
                if (!imageTypes.Contains(produto.img_produto.ContentType))
                {
                    ModelState.AddModelError("img_produto", "Escolha uma imagem GIF, JPG ou PNG.");
                }
            }
            if (produto.produto_nome == null)
            {
                ModelState.AddModelError("produto_nome", "Nome do produto deve ser preenchido.");
                return View(produto);
            }
            if (produto.produto_nome != null)
            {
                if (produto.produto_nome.Length > 50)
                {
                    ModelState.AddModelError("produto_nome", "Nome do produto deve ser somente até 50 caracteres.");
                    return View(produto);
                }
            }
            if (produto.produto_preco == null)
            {
                ModelState.AddModelError("produto_preco", "Preco do produto deve ser preenchido.");
                return View(produto);
            }
            if (produto.produto_desc != null)
            {
                if (produto.produto_desc.Length > 500)
                {
                    ModelState.AddModelError("produto_desc", "Descrição é muito grande.");
                    return View(produto);
                }
            }
            if (ModelState.IsValid)
            {
                if (produto.produto_ativo == null)
                {
                    produto.produto_ativo = "N";
                }
                if (produto.img_produto != null)
                {
                    var imagemNome = String.Format("{0:yyyyMMdd-HHmmssfff}", DateTime.Now);
                    var extensao = System.IO.Path.GetExtension(produto.img_produto.FileName).ToLower();
                    var file = String.Format(imagemNome, extensao);
                    using (var img = System.Drawing.Image.FromStream(produto.img_produto.InputStream))
                    {
                        produto.produto_img = String.Format("/Imagem/Produto/{0}{1}", imagemNome, extensao);
                        var produto_img_img = String.Format("~/Imagem/Produto/{0}{1}", imagemNome, extensao);
                        // Salva imagem 
                        SalvarNaPasta(img, produto_img_img);
                    }
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

        private void SalvarNaPasta(Image img, string caminho)
        {
            /*Bitmap myBitmap;
            ImageCodecInfo myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;*/
            // Obtém a nova resolução
            //Size tamanhoImagem = NovoTamanhoImagem(img.Size, novoTamanho);
            using (Image novaImagem = new Bitmap(img))
             {
                 var x = Server.MapPath(caminho);
                var j = caminho;
                 novaImagem.Save(x, img.RawFormat);
             }
            /*myEncoder = Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, 75L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            myImageCodecInfo = GetEncoderInfo("image/jpeg");
            myBitmap = new Bitmap(img);
            myBitmap.Save(nome, myImageCodecInfo, myEncoderParameters);*/

        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        /*
         // Save the bitmap as a JPEG file with quality level 75.
    myEncoderParameter = new EncoderParameter(myEncoder, 75L);
    myEncoderParameters.Param[0] = myEncoderParameter;
    myImageCodecInfo = GetEncoderInfo("image/jpeg");
    myBitmap = new Bitmap("Shapes.bmp");
    myBitmap.Save("Shapes075.jpg", myImageCodecInfo, myEncoderParameters);


     myBitmap = new Bitmap("Shapes.bmp");

    // Get an ImageCodecInfo object that represents the JPEG codec.


    // Create an Encoder object based on the GUID

    // for the Quality parameter category.
    myEncoder = Encoder.Quality;
         */
    
        //Save(String, ImageFormat)	
        //Salva essa Image no arquivo especificado no formato especificado.
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
