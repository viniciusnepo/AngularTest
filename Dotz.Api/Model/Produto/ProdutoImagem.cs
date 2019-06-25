using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dotz.Api
{
    public class ProdutoImagem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProdutoImagem { get; set; }

        [Required]
        public int IdProduto { get; set; }

        [Required]
        public string URL { get; set; }
    }
}
