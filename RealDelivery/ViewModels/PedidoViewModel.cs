using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealDelivery.ViewModels
{
    public class PedidoViewModel
    {

        public Pedido ped { get; set; }

    }

    public class ItensPedidoViewModel
    {
        public ItemPedido iped { get; set; }
    }
}