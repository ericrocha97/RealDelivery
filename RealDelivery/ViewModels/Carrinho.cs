using RealDelivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealDelivery.ViewModels
{
    public class Carrinho
    {

        private readonly List<ItemCarrinho> _itemCarrinho = new List<ItemCarrinho>();

        //ADD
        public void AdicionarItem(produto pro, int qtd)
        {
            ItemCarrinho item = _itemCarrinho.FirstOrDefault(p => p.Produto.produto_cod == pro.produto_cod);
            if (item == null)
            {
                _itemCarrinho.Add(new ItemCarrinho
                {
                    Produto = pro,
                    Quantidade = qtd
                });
            }
            else
            {
                item.Quantidade += qtd;
            }
        }

        //REMOVE
        public void RemevorItem(produto pro)
        {
            _itemCarrinho.RemoveAll(l => l.Produto.produto_cod == pro.produto_cod);
        }

        //TOTAL
        public float? ObterValorTotal()
        {
            return _itemCarrinho.Sum(e => e.Produto.produto_preco * e.Quantidade);
        }

        //CLEAR
        public void LimparCarrinho()
        {
            _itemCarrinho.Clear();
        }

        //RETURN
        public IEnumerable<ItemCarrinho> ItensCarrinho
        {
            get { return _itemCarrinho; }
        }
    }

    public class ItemCarrinho
    {
        public produto Produto { get; set; }
        public int Quantidade { get; set; }
    }
}