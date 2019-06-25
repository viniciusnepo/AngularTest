using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dotz.Api
{
    public class ProdutoCategoria
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCategoria { get; set; }

        [Required]
        public string Nome { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual ICollection<ProdutoSubcategoria> Subcategorias { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Ativo { get; set; }
    }
}
