using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dotz.Api
{
    public class ConsumidorMovimentacao
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdConsumidorMovimentacao { get; set; }

        [Required]
        public int IdConsumidor { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public Natureza Natureza { get; set; }

        [Required]
        public double Valor { get; set; }

        public int? IdPedidoItem { get; set; } //Será preenchido se for operação de débito

        [ForeignKey("IdPedidoItem")]
        public virtual PedidoItem Pedido { get; set; }

        public int? IdParceiroCredito { get; set; } //Será preenchido se for operação de crédito

        [ForeignKey("IdParceiroCredito")]
        public virtual ParceiroCredito Credito { get; set; }
    }

    public enum Natureza { Credito = 1, Debito = 2 }
}
