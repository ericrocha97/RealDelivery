using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealDelivery;
using RealDelivery.ViewModels;
//RealDelivery.ViewModels.CarrinhoViewModel
namespace RealDelivery.Controllers
{
    public class PedidoController : Controller
    {
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();

        public PedidoViewModel ObterPedido()
        {

            PedidoViewModel PedidoView = (PedidoViewModel)Session["PedidoView"];
            if (PedidoView == null)
            {
                PedidoView = new PedidoViewModel();
                Session["PedidoView"] = PedidoView;
            }

            return PedidoView;
        }
        public List<ItensPedidoViewModel> _ItemPedido;

        [Authorize(Roles = "Cliente")]
        [HttpGet]
        public ActionResult ConcluirPedido([Bind(Include = "car")]CarrinhoViewModel Carrinho)
        {
            if (User.Identity.IsAuthenticated)
            {
                //ObterPedido().iped.item_pedido_cod = Carrinho.car.ItensCarrinho()
                var PedidoView = new PedidoViewModel();
                var itensPed = new ItensPedidoViewModel();
                
                foreach(var item in Carrinho.car.ItensCarrinho)
                {
                    /*
                        public int item_pedido_cod { get; set; }
                        public int pedido_cod { get; set; }
                        public int produto_cod { get; set; }
                        public int item_pedido_qtd { get; set; }
                        public float produto_valor { get; set; }
                        public string produto_nome { get; set; }
                     */

                    //_ItemPedido.Add()
                    itensPed.iped.produto_cod = item.Produto.produto_cod;
                    itensPed.iped.produto_nome = item.Produto.produto_nome;
                    itensPed.iped.produto_valor = (float)item.Produto.produto_preco;
                    itensPed.iped.item_pedido_qtd = item.Quantidade;
                    _ItemPedido.Add(itensPed);


                }
                
                //ObterPedido().ped._ItemPedido.Add() = Carrinho.car.ItensCarrinho;
                var total = Carrinho.car.ObterValorTotal();
                var userName = User.Identity.Name;
                var cliente = db.cliente.FirstOrDefault(x => x.cliente_email == userName);

                if (cliente != null)
                {
                    int? id = cliente.cliente_cod;
                    if (id == null)
                    {
                        return View("index", "Carrinho");
                    }
                    var endereco = db.endereco.Where(e => e.cliente_cod == id);
                    if (endereco == null)
                    {
                        return View("index", "Carrinho");
                    }


                    return View(endereco.ToList());
                }



                /*
                    PedidoView.ped.cliente_cod = cliente.cliente_cod;
                    PedidoView.ped.cliente_nome = cliente.cliente_nome;
                    PedidoView.ped.pedido_valor = (float)total;    
             */

            }

            return View("index", "Carrinho");

        }
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public ActionResult ConcluirPedido([Bind(Include = "pedido_cod,cliente_cod,pedido_data,pedido_valor,pedido_obs,pedido_ent,endereco_cod,pedido_fpgto,pedido_troco")] pedido pedido)
        {


            return View();
        }

        // GET: Pedido
        public ActionResult Index()
        {
            var pedido = db.pedido.Include(p => p.cliente).Include(p => p.endereco);
            return View(pedido.ToList());
        }

        // GET: Pedido/Details/5
        public ActionResult Details(int? id)
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

        // GET: Pedido/Create
        public ActionResult Create()
        {
            ViewBag.cliente_cod = new SelectList(db.cliente, "cliente_cod", "cliente_nome");
            ViewBag.endereco_cod = new SelectList(db.endereco, "endereco_cod", "endereco_cep");
            return View();
        }

        // POST: Pedido/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pedido_cod,cliente_cod,pedido_data,pedido_valor,pedido_obs,pedido_ent,endereco_cod,pedido_fpgto,pedido_troco")] pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.pedido.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cliente_cod = new SelectList(db.cliente, "cliente_cod", "cliente_nome", pedido.cliente_cod);
            ViewBag.endereco_cod = new SelectList(db.endereco, "endereco_cod", "endereco_cep", pedido.endereco_cod);
            return View(pedido);
        }

        // GET: Pedido/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.cliente_cod = new SelectList(db.cliente, "cliente_cod", "cliente_nome", pedido.cliente_cod);
            ViewBag.endereco_cod = new SelectList(db.endereco, "endereco_cod", "endereco_cep", pedido.endereco_cod);
            return View(pedido);
        }

        // POST: Pedido/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pedido_cod,cliente_cod,pedido_data,pedido_valor,pedido_obs,pedido_ent,endereco_cod,pedido_fpgto,pedido_troco")] pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cliente_cod = new SelectList(db.cliente, "cliente_cod", "cliente_nome", pedido.cliente_cod);
            ViewBag.endereco_cod = new SelectList(db.endereco, "endereco_cod", "endereco_cep", pedido.endereco_cod);
            return View(pedido);
        }

        // GET: Pedido/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pedido pedido = db.pedido.Find(id);
            db.pedido.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("Index");
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
