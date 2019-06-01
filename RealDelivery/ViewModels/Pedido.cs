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
        public List<ItemPedido> _ItemPedido { get; set; }
        //pedido
        public int pedido_cod { get; set; }
        public System.DateTime pedido_data { get; set; }
        public float pedido_valor { get; set; }
        public string pedido_obs { get; set; }
        public string pedido_ent { get; set; }
        public string pedido_fpgto { get; set; }
        public float pedido_troco { get; set; }
        //cliente
        public int cliente_cod { get; set; }
        public string cliente_nome { get; set; }
        public string cliente_telefone { get; set; }
        public string cliente_email { get; set; }
        //endereco
        public int endereco_cod { get; set; }
        public string endereco_cep { get; set; }
        public string endereco_rua { get; set; }
        public string endereco_bairro { get; set; }
        public string endereco_cidade { get; set; }
        public string endereco_uf { get; set; }
        public string endereco_comp { get; set; }
        public string endereco_num { get; set; }


    }

    public class ItemPedido
    {
       // public int item_pedido_cod { get; set; }
        //public int pedido_cod { get; set; }
        public int produto_cod { get; set; }
        public int item_pedido_qtd { get; set; }
        public float produto_valor { get; set; }
        public string produto_nome { get; set; }
    }
}