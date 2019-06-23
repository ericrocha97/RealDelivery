using RealDelivery.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealDelivery.Controllers
{
    public class CarrinhoController : Controller
    {
        private db_a464fd_realdevEntities db = new db_a464fd_realdevEntities();

        public Carrinho ObterCarrinho()
        {
            Carrinho carrinho = (Carrinho)Session["Carrinho"];
            if (carrinho == null)
            {
                carrinho = new Carrinho();
                Session["Carrinho"] = carrinho;
            }
            return carrinho;
        }

        // GET: Carrinho
        public ViewResult Index()
        {
            return View(new CarrinhoViewModel
            {
                car = ObterCarrinho()
            });
        }

        //GET
        [HttpGet]
        public ActionResult add_prod(int id_prod, int quantidade)
        {
            produto produto = db.produto.Find(id_prod);
            ObterCarrinho().AdicionarItem(produto, quantidade);
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public ActionResult remove(int id_prod)
        {
            produto produto = db.produto.Find(id_prod);
            ObterCarrinho().RemevorItem(produto);
            return RedirectToAction("index");
        }
        public ActionResult removeOne(int id_prod, int quantidade)
        {
            produto produto = db.produto.Find(id_prod);
            ObterCarrinho().Remevor(produto, quantidade);
            return RedirectToAction("index");
        }
        public ActionResult addOne(int id_prod, int quantidade)
        {
            produto produto = db.produto.Find(id_prod);
            ObterCarrinho().AdicionarItem(produto, quantidade);
            return RedirectToAction("index");
        }

        [HttpGet]
        public ActionResult limpa()
        {
            ObterCarrinho().LimparCarrinho();
            return RedirectToAction("index", "home");
        }
    }
}

