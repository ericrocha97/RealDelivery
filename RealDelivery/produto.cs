//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealDelivery
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web;

    public partial class produto
    {
        public long produto_cod { get; set; }
        [DisplayName("Nome do Produto")]
        public string produto_nome { get; set; }
        [DisplayName("Grupo do produto")]
        public long grupo_cod { get; set; }
        [DisplayName("Pre�o do produto")]
        public Nullable<float> produto_preco { get; set; }
        [DisplayName("Pre�o de custo")]
        public Nullable<float> produto_custo { get; set; }
        [DisplayName("Ativo")]
        public string produto_ativo { get; set; }
        [DisplayName("Impressora do produto")]
        public Nullable<long> impress_cod { get; set; }
        [DisplayName("Descri��o do produto")]
        public string produto_desc { get; set; }
        [DisplayName("Imagem do produto")]
        public string produto_img { get; set; }

        public HttpPostedFileBase img_produto { get; set; }

        public virtual grupo grupo { get; set; }
        public virtual impress impress { get; set; }
    }
}
