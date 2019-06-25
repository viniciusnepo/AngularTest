using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotz.Api
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Papel { get; set; }
        public string Token { get; set; }
    }
}
