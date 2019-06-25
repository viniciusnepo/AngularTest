using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dotz.Api
{
    public class ProdutoSubcategoria
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSubcategoria { get; set; }

        [Required]
        public int IdCategoria { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Ativo { get; set; }

        [ForeignKey("IdSubcategoria")]
        public virtual ICollection<Produto> Produtos { get; set; }
    }
}
