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
    using System.ComponentModel.DataAnnotations;

    public partial class cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cliente()
        {
            this.endereco = new HashSet<endereco>();
            this.pedido = new HashSet<pedido>();
        }

        public int cliente_cod { get; set; }
        [DisplayName("Nome")]
        public string cliente_nome { get; set; }
        [DisplayName("Telefone")]
        [DisplayFormat(DataFormatString = "{0:(##) #####-####}")]
        public string cliente_telefone { get; set; }
        [DisplayName("E-mail")]
        public string cliente_email { get; set; }
        [DisplayName("Senha")]
        public string cliente_senha { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<endereco> endereco { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<pedido> pedido { get; set; }
    }
}
