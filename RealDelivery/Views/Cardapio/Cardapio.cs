using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalCardapio.Models
{
    public class Cardapio
    {
        public int grupo_cod { get; set; }

        public string grupo_nome { get; set; }

        public string grupo_desc { get; set; }

        public string grupo_img { get; set; }
    }
}