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
    
    public partial class produto
    {
        public long produto_cod { get; set; }
        public string produto_nome { get; set; }
        public long grupo_cod { get; set; }
        public Nullable<float> produto_preco { get; set; }
        public Nullable<float> produto_custo { get; set; }
        public string produto_ativo { get; set; }
        public Nullable<long> impress_cod { get; set; }
        public string produto_desc { get; set; }
        public string produto_img { get; set; }
    
        public virtual grupo grupo { get; set; }
        public virtual impress impress { get; set; }
    }
}
