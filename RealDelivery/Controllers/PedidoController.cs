using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
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
        private readonly db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();
        //public float? total;
        private Carrinho ObterCarrinho()
        {
            Carrinho carrinho = (Carrinho)Session["Carrinho"];
            if (carrinho == null)
            {
                carrinho = new Carrinho();
                Session["Carrinho"] = carrinho;
            }
            return carrinho;
        }
        public Pedido ObterPedido()
        {
            Pedido pedido = (Pedido)Session["pedido"];
            if (pedido == null)
            {
                pedido = new Pedido();
                Session["pedido"] = pedido;
            }
            return pedido;
        }
        public ItensPedidoViewModel ObterItemPedido()
        {
            ItensPedidoViewModel PedidoItensView = (ItensPedidoViewModel)Session["PedidoItensView"];
            if (PedidoItensView == null)
            {
                PedidoItensView = new ItensPedidoViewModel();
                Session["PedidoItensView"] = PedidoItensView;
            }
            return PedidoItensView;
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult ConcluirPedido()
        {
            var Carrinho = new CarrinhoViewModel
            {
                car = ObterCarrinho()
            };
            if (User.Identity.IsAuthenticated)
            {
                var PedidoView = new PedidoViewModel
                {
                    ped = new Pedido()
                };
                ObterItemPedido();
                var itensPed = new ItemPedido();
                foreach (var item in Carrinho.car.ItensCarrinho)
                {
                    itensPed.Produto = item.Produto;
                    itensPed.Quantidade = item.Quantidade;
                    var itempedido = new ItensPedidoViewModel
                    {
                        iped = itensPed
                    };
                    ObterPedido().AdicionarItem(itempedido.iped.Produto, itempedido.iped.Quantidade);
                    //PedidoView.ped._ItemPedido.Add(itempedido.iped);
                }
                //ObterPedido().ped._ItemPedido.Add() = Carrinho.car.ItensCarrinho;
                var total = Carrinho.car.ObterValorTotal();
                var teste = ObterPedido();
                var userName = User.Identity.Name;
                var cliente = db.cliente.FirstOrDefault(x => x.cliente_email == userName);
                ObterPedido().Cliente = cliente;
                if (cliente != null)
                {
                    int? id = cliente.cliente_cod;
                    if (id == null)
                    {
                        return RedirectToAction("index", "Carrinho");

                    }
                    var endereco = db.endereco.Where(e => e.cliente_cod == id);
                    if (endereco == null)
                    {
                        return RedirectToAction("index", "Carrinho");
                    }
                    return View(endereco.ToList());
                }
            }
            return RedirectToAction("index", "Carrinho");
        }
        [Authorize(Roles = "Cliente")]
        public ActionResult EscolheEndereco(int? id)
        {
            ObterPedido().Endereco = db.endereco.Find(id);
            return View("ConcluirPedidoNext");
        }
        [Authorize(Roles = "Cliente")]
        public ActionResult FinalizarPedido([Bind(Include = "pedido_cod,cliente_cod,pedido_data,pedido_valor,pedido_obs,pedido_ent,endereco_cod,pedido_fpgto,pedido_troco")] pedido pedido)
        {
            //var Pedido = new pedido();
            pedido.cliente_cod = ObterPedido().Cliente.cliente_cod;
            pedido.endereco_cod = ObterPedido().Endereco.endereco_cod;
            pedido.pedido_data = DateTime.Now;
            var aux = pedido.pedido_data;
            pedido.pedido_ent = "S";
            pedido.pedido_fpgto = "3";
            pedido.pedido_obs = "";
            pedido.pedido_troco = 0;
            pedido.pedido_valor = (float)ObterCarrinho().ObterValorTotal();
            var item_pedido = new item_pedido();
            ObterCarrinho().LimparCarrinho();
            try
            {
                if (ModelState.IsValid)
                {
                    db.pedido.Add(pedido);
                    db.SaveChanges();
                    var data = db.pedido.Max(x => x.pedido_cod);
                    {
                        var itens = ObterPedido()._ItemPedido;
                        foreach (var item in itens)
                        {
                            item_pedido.pedido_cod = data;
                            item_pedido.produto_cod = item.Produto.produto_cod;
                            item_pedido.item_pedido_qtd = item.Quantidade;
                            item_pedido.produto_valor = (float)item.Produto.produto_preco;
                            item_pedido.produto_nome = item.Produto.produto_nome;
                            db.item_pedido.Add(item_pedido);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Panel", "Cliente");
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var dbValidationError in entityValidationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation(" PropertyName: {0} ErrorMessage: {1} ", dbValidationError.PropertyName, dbValidationError.ErrorMessage);
                    }
                }
            };
            return RedirectToAction("Panel", "Cliente");
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
