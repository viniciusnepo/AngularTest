using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotz.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParceiroController : ControllerBase
    {
        private readonly DzContext _context;

        public ParceiroController(DzContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = Papel.Admin)]
        public IEnumerable<Parceiro> GetParceiros()
        {
            return _context.Parceiros;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> GetParceiro([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parceiro = await _context.Parceiros.FindAsync(id);

            if (parceiro == null)
            {
                return NotFound();
            }

            return Ok(parceiro);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Papel.Parceiro + "," + Papel.Admin)]
        public async Task<IActionResult> PutParceiro([FromRoute] int id, [FromBody] Parceiro parceiro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parceiro.IdParceiro)
            {
                return BadRequest();
            }

            var parceiroOriginal = _context.Parceiros.AsNoTracking().FirstOrDefault(x => x.IdParceiro == id);
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == parceiroOriginal.Email);

            usuario.Email = parceiro.Email;
            usuario.Nome = parceiro.RazaoSocial;
            usuario.Senha = parceiro.Senha;

            _context.Entry(usuario).State = EntityState.Modified;
            _context.Entry(parceiro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParceiroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostParceiro([FromBody] Parceiro parceiro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Verifica se já possui usuário cadastrado com mesmo email
            if (_context.Usuarios.FirstOrDefault(x => x.Email == parceiro.Email) != null)
            {
                return BadRequest(new { message = "Já existe usuário com esse email!" });
            }

            //Verifica se já possui parceiro cadastrado com mesmo CNPJ
            if (_context.Parceiros.FirstOrDefault(x => x.CNPJ == parceiro.CNPJ) != null)
            {
                return BadRequest(new { message = "Já existe usuário com esse CNPJ!" });
            }

            Usuario novoUser = new Usuario()
            {
                Email = parceiro.Email,
                Nome = parceiro.RazaoSocial,
                Senha = parceiro.Senha,
                Papel = Api.Papel.Parceiro
            };

            _context.Parceiros.Add(parceiro);
            _context.Usuarios.Add(novoUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParceiro", new { id = parceiro.IdParceiro }, parceiro);
        }

        [HttpPost("adicionarCredito")]
        [Authorize(Roles = Papel.Parceiro + "," + Papel.Admin)]
        public async Task<IActionResult> AdicionarCreditoUsuario([FromBody] SolicitacaoCredito solicitacaoCredito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Consumidor consumidor = _context.Consumidores.FirstOrDefault(x => x.Email == solicitacaoCredito.EmailConsumidor);
            if (consumidor == null)
            {
                return BadRequest(new { message = "Não existe consumidor cadastrado com esse email!" });
            }

            ParceiroCredito credito = new ParceiroCredito()
            {
                IdParceiro = solicitacaoCredito.IdParceiro,
                Data = DateTime.Now,
                ValorDZ = solicitacaoCredito.ValorDZ
            };
            credito.IdConsumidor = consumidor.IdConsumidor;

            ConsumidorMovimentacao movimentacao = new ConsumidorMovimentacao()
            {
                IdConsumidor = consumidor.IdConsumidor,
                Data = DateTime.Now,
                Natureza = Natureza.Credito,
                Valor = credito.ValorDZ,
                Credito = credito
            };
            _context.ConsumidorMovimentacoes.Add(movimentacao);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Crédito Lançado com sucesso!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParceiro([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parceiro = await _context.Parceiros.FindAsync(id);
            if (parceiro == null)
            {
                return NotFound();
            }

            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == parceiro.Email);

            _context.Usuarios.Remove(usuario);
            _context.Parceiros.Remove(parceiro);
            await _context.SaveChangesAsync();

            return Ok(parceiro);
        }

        private bool ParceiroExists(int id)
        {
            return _context.Parceiros.Any(e => e.IdParceiro == id);
        }
    }

    public class SolicitacaoCredito
    {
        public int IdParceiro { get; set; }

        public string EmailConsumidor { get; set; }

        public double ValorDZ { get; set; }
    }
}