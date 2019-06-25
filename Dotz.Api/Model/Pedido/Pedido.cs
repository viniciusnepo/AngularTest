using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dotz.Api
{
    public class Pedido
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPedido { get; set; }

        [Required]
        public int IdConsumidor { get; set; }

        [ForeignKey("IdConsumidor")]
        public virtual Consumidor Consumidor { get; set; }

        public StatusPedido Status { get; set; }

        [Required]
        public DateTime DataPedido { get; set; }

        public DateTime? DataSeparacao { get; set; }

        public DateTime? DataEmissaoNF { get; set; }

        public DateTime? DataEnvio { get; set; }

        public DateTime? DataRecebimento { get; set; }

        public DateTime? DataCancelamento { get; set; }

        public string Observacoes { get; set; }

        public string NumeroNF { get; set; }

        public string CodigoRastreio { get; set; }

        [ForeignKey("IdPedido")]
        public virtual ICollection<PedidoItem> Itens { get; set; }

        [MaxLength(8)]
        [Required]
        public string CEP { get; set; }

        [Required]
        public string Logradouro { get; set; }

        [MaxLength(20)]
        [Required]
        public string Numero { get; set; }

        public string Complemento { get; set; }

        [MaxLength(20)]
        [Required]
        public string Bairro { get; set; }

        [MaxLength(20)]
        [Required]
        public string Cidade { get; set; }

        [MaxLength(2)]
        [Required]
        public string UF { get; set; }
    }

    public enum StatusPedido { Realizado = 1, EmSeparacao = 2, NotaEmitida = 3, Enviado = 4, Recebido = 5, Cancelado = 6 }
}
