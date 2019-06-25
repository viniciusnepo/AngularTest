using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotz.Api
{
    public class Consumidor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdConsumidor { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Sobrenome { get; set; }

        [Required]
        public Sexo Sexo { get; set; }

        [Required]
        public DateTime DtNascimento { get; set; }

        public string TelefoneResidencial { get; set; }

        [Required]
        public string TelefoneCelular { get; set; }

        [ForeignKey("IdConsumidor")]
        public virtual ICollection<ConsumidorEndereco> Enderecos { get; set; }

        [ForeignKey("IdConsumidor")]
        public virtual ICollection<ConsumidorMovimentacao> Movimentacoes { get; set; }
    }

    public enum Sexo { Masculino = 1, Feminino = 2 }
}
