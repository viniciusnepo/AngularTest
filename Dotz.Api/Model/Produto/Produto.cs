using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dotz.Api
{
    public class Produto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProduto { get; set; }

        [Required]
        public int IdSubcategoria { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public string Marca { get; set; }

        [Required]
        public string EAN { get; set; }

        [Required]
        [DefaultValue(0)]
        public int QuantidadeEstoque { get; set; }

        [Required]
        public double ValorDotz { get; set; }

        public double? ValorDotzPromocional { get; set; }

        [ForeignKey("IdProduto")]
        public virtual ICollection<ProdutoImagem> Imagens { get; set; }
    }
}
