using RealDelivery;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RealDelivery.ViewModels
{
    
    public class Pedido
    {
        //public List<ItemPedido> _ItemPedido { get; set; }
        public List<ItemPedido> _ItemPedido = new List<ItemPedido>();
        //ADD
        public void AdicionarItem(produto pro, int qtd)
        {
            ItemPedido item = _ItemPedido.FirstOrDefault(p => p.Produto.produto_cod == pro.produto_cod);
            if (item == null)
            {
                _ItemPedido.Add(new ItemPedido
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
        public void LimparPedido()
        {
            _ItemPedido.Clear();
        }
        //pedido
        public pedido _Pedido { get; set; }
        //cliente
        public cliente Cliente { get; set; }
        //endereco
        public endereco Endereco { get; set; }


    }

    public class ItemPedido
    {

        public produto Produto { get; set; }
        public int Quantidade { get; set; }
    }
}