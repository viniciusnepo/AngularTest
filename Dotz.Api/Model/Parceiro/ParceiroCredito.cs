using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dotz.Api
{
    public class ParceiroCredito
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdParceiroCredito { get; set; }

        public int IdParceiro { get; set; }

        [ForeignKey("IdParceiro")]
        public virtual Parceiro Parceiro { get; set; }

        public int IdConsumidor { get; set; }

        [ForeignKey("IdConsumidor")]
        public virtual Consumidor Consumidor { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public double ValorDZ { get; set; }

        public double? ValorMoedaOriginal { get; set; }

        public Moeda? MoedaOriginal { get; set; }
    }
}
