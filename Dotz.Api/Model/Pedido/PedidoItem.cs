using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dotz.Api
{
    public class PedidoItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPedidoItem { get; set; }

        [Required]
        public int IdPedido { get; set; }
               
        [Required]
        public int IdProduto { get; set; }

        [ForeignKey("IdProduto")]
        public virtual Produto Produto { get; set; }

        [DefaultValue(1)]
        public int Quantidade { get; set; }

        [Required]
        public double ValorUnitarioDZ { get; set; }
    }
}
