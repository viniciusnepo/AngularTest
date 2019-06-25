using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dotz.Api;
using Microsoft.AspNetCore.Authorization;

namespace Dotz.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ConsumidorController : ControllerBase
    {
        private readonly DzContext _context;

        public ConsumidorController(DzContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = Papel.Admin)]
        public IEnumerable<Consumidor> GetConsumidores()
        {
            return _context.Consumidores.Include(x => x.Enderecos);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> GetConsumidor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var consumidor = await _context.Consumidores.FindAsync(id);

            if (consumidor == null)
            {
                return NotFound();
            }

            return Ok(consumidor);
        }

        [HttpGet("saldo/{id}")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public double SaldoConsumidor([FromRoute] int id)
        {
            var consumidor = _context.Consumidores.Find(id);

            if (consumidor == null)
            {
                return 0;
            }

            double saldo = _context.ConsumidorMovimentacoes.Where(x => x.IdConsumidor == id).Sum(x => x.Valor);

            return saldo;
        }

        [HttpGet("extrato/{id}")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public IEnumerable<ConsumidorMovimentacao> ExtratoConsumidor([FromRoute] int id)
        {
            return _context.ConsumidorMovimentacoes
                .Where(x => x.IdConsumidor == id)
                .Include(x => x.Pedido)
                .Include("Pedido.Produto")
                .Include(x => x.Credito)
                .Include("Credito.Parceiro")
                .OrderBy(x => x.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public async Task<IActionResult> PutConsumidor([FromRoute] int id, [FromBody] Consumidor consumidor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != consumidor.IdConsumidor)
            {
                return BadRequest();
            }

            var consumidorOriginal = _context.Consumidores.AsNoTracking().FirstOrDefault(x => x.IdConsumidor == id);
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == consumidorOriginal.Email);

            usuario.Email = consumidor.Email;
            usuario.Nome = consumidor.Nome + " " + consumidor.Sobrenome;
            usuario.Senha = consumidor.Senha;

            _context.Entry(usuario).State = EntityState.Modified;
            _context.Entry(consumidor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsumidorExists(id))
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
        public async Task<IActionResult> PostConsumidor([FromBody] Consumidor consumidor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Verifica se já possui usuário cadastrado com mesmo email
            if (_context.Usuarios.FirstOrDefault(x => x.Email == consumidor.Email) != null)
            {
                return BadRequest(new { message = "Já existe usuário com esse email!" });
            }

            //Verifica se já possui consumidor cadastrado com mesmo CPF
            if (_context.Consumidores.FirstOrDefault(x => x.CPF == consumidor.CPF) != null)
            {
                return BadRequest(new { message = "Já existe usuário com esse CPF!" });
            }

            Usuario novoUser = new Usuario()
            {
                Email = consumidor.Email,
                Nome = consumidor.Nome + " " + consumidor.Sobrenome,
                Senha = consumidor.Senha,
                Papel = Api.Papel.Consumidor
            };

            _context.Consumidores.Add(consumidor);
            _context.Usuarios.Add(novoUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConsumidor", new { id = consumidor.IdConsumidor }, consumidor);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public async Task<IActionResult> DeleteConsumidor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var consumidor = await _context.Consumidores.FindAsync(id);
            if (consumidor == null)
            {
                return NotFound();
            }

            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == consumidor.Email);

            _context.Usuarios.Remove(usuario);
            _context.Consumidores.Remove(consumidor);
            await _context.SaveChangesAsync();

            return Ok(consumidor);
        }

        [HttpGet("Endereco/{idConsumidor}")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public IEnumerable<ConsumidorEndereco> GetEnderecos([FromRoute] int idConsumidor)
        {
            return _context.ConsumidorEnderecos.Where(x => x.IdConsumidor == idConsumidor);
        }

        [HttpPost("Endereco")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public async Task<IActionResult> InserirEndereco([FromBody] ConsumidorEndereco endereco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Consumidor consumidor = _context.Consumidores.Include(x => x.Enderecos).FirstOrDefault(x => x.IdConsumidor == endereco.IdConsumidor);
            if (consumidor == null)
            {
                return NotFound();
            }

            //Se marcou endereço como principal, deixa os outros como secundários
            if (endereco.Principal)
            {
                foreach (var end in consumidor.Enderecos)
                {
                    end.Principal = false;
                }
            }

            consumidor.Enderecos.Add(endereco);

            await _context.SaveChangesAsync();

            return Ok(endereco);
        }

        [HttpPut("Endereco/{id}")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public async Task<IActionResult> AlterarEndereco([FromRoute] int id, [FromBody] ConsumidorEndereco endereco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != endereco.IdConsumidorEndereco)
            {
                return BadRequest();
            }

            //Verifica se existe o endereço dentro do consumidor
            Consumidor consumidor = _context.Consumidores.AsNoTracking().Include(x => x.Enderecos).FirstOrDefault(x => x.IdConsumidor == endereco.IdConsumidor);
            if (consumidor.Enderecos.Count(x => x.IdConsumidorEndereco == id) == 0)
            {
                return NotFound();
            }

            //Se marcou endereço como principal, deixa os outros como secundários
            if (endereco.Principal)
            {
                List<ConsumidorEndereco> lstEnderecosCons = _context.ConsumidorEnderecos.Where(x => x.IdConsumidor == endereco.IdConsumidor && x.IdConsumidorEndereco != endereco.IdConsumidorEndereco).ToList();
                lstEnderecosCons.ForEach(x => x.Principal = false);
            }

            _context.Entry(endereco).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(endereco);
        }

        [HttpDelete("Endereco/{id}")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public async Task<IActionResult> DeleteEndereco([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var endereco = await _context.ConsumidorEnderecos.FindAsync(id);
            if (endereco == null)
            {
                return NotFound();
            }
            _context.ConsumidorEnderecos.Remove(endereco);
            await _context.SaveChangesAsync();

            return Ok(endereco);
        }

        private bool ConsumidorExists(int id)
        {
            return _context.Consumidores.Any(e => e.IdConsumidor == id);
        }
    }
}