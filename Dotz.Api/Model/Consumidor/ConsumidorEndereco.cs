using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dotz.Api
{
    [Table("ConsumidorEndereco")]
    public class ConsumidorEndereco
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdConsumidorEndereco { get; set; }

        [Required]
        public int IdConsumidor { get; set; }

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

        [Required]
        [DefaultValue(false)]
        public bool Principal { get; set; }
    }
}
