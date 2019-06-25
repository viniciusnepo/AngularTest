using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dotz.Api
{
    public class Parceiro
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdParceiro { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public string CNPJ { get; set; }

        [Required]
        public string RazaoSocial { get; set; }

        [Required]
        public string NomeFantasia { get; set; }

        public string LogoURL { get; set; }

        [Required]
        public string Regulamento { get; set; }


        //Criei esse FatorDZ e MoedaTroca pois observei que no site da Dotz, cada parceiro tem um valor recebido em DZ diferente, sendo alguns em moedas diferentes
        [Required]
        [DefaultValue(1)]
        public int FatorDZ { get; set; }

        [Required]
        [DefaultValue(1)]
        public Moeda MoedaTroca { get; set; }

        [Required]
        public string URLRedirecionamento { get; set; }
    }

    public enum Moeda { Real = 1, Dolar = 2, Euro = 3 }
}
