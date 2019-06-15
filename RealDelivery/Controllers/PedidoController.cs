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
            if (id == 0)
            {
                // ObterPedido().Endereco.endereco_cod = 0;
                //ObterPedido()._Pedido.pedido_ent = "0";
                return View("ConcluirPedidoNext");
            }
            else
            {
                ObterPedido().Endereco = db.endereco.Find(id);
                //ObterPedido()._Pedido.pedido_ent = "1";
                return View("ConcluirPedidoNext");
            }
        }
        //
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public ActionResult FinalizarPedido([Bind(Include = "pedido_cod,cliente_cod,pedido_data,pedido_valor,pedido_obs,pedido_ent,endereco_cod,pedido_fpgto,pedido_troco")] pedido pedido)
        {
            //var Pedido = new pedido();
            pedido.cliente_cod = ObterPedido().Cliente.cliente_cod;
            if(ObterPedido().Endereco == null)
            {
                pedido.endereco_cod = null;
                pedido.pedido_ent = "0";
            }
            else
            {
                pedido.endereco_cod = ObterPedido().Endereco.endereco_cod;
                pedido.pedido_ent = "1";
            }
            
            pedido.pedido_data = DateTime.Now;
            var aux = pedido.pedido_data;

            pedido.pedido_valor = (float)ObterCarrinho().ObterValorTotal();
            /*if(pedido.pedido_fpgto == "0"){
                if(pedido.pedido_troco != null){
                    if(pedido.pedido_troco <= pedido.pedido_valor){
                        //Mensagem dizendo q o troco tem q ser maior q o valor total do pedido.
                        if(pedido.endereco_cod == null)
                        {
                            //ModelState.AddModelError("pedido_troco", "O valor do troco deve ser maior que o valor total do pedido: " + pedido.pedido_valor);

                            
                            ViewBag.Message = "O valor do troco deve ser maior que o valor total do pedido: " + pedido.pedido_valor;
                            return RedirectToAction("EscolheEndereco", "Pedido", "0");
                        }
                        else
                        {
                            //ModelState.AddModelError("pedido_troco", "O valor do troco deve ser maior que o valor total do pedido: " + pedido.pedido_valor);
                            //
                            ViewBag.Message = "O valor do troco deve ser maior que o valor total do pedido: " + pedido.pedido_valor;
                            return RedirectToAction("EscolheEndereco", "Pedido", pedido.endereco_cod);
                        }

                    }
                }
            }*/

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
                        ObterPedido().LimparPedido();
                    }
                    //return RedirectToAction("Pedidos", "Cliente", pedido.cliente_cod);
                    return Redirect("~/Cliente/Pedidos/" + pedido.cliente_cod);

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
            //return RedirectToAction("Pedidos", "Cliente", pedido.cliente_cod,);
            return Redirect("~/Cliente/Pedidos/"+ pedido.cliente_cod);
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
