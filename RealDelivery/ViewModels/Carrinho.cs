using RealDelivery;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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

        public void Remevor(produto pro)
        {
            var item = _itemCarrinho.Find(l => l.Produto.produto_cod == pro.produto_cod);
            _itemCarrinho.Remove(item);
        }

        //TOTAL
        //[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public float? ObterValorTotal()
        {
            var resultado = _itemCarrinho.Sum(e => e.Produto.produto_preco * e.Quantidade);
            //var valorFormatado = string.Format("{0:C}", resultado.ToString());
            //var valorFormatado = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", resultado);
            return resultado;
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