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
    public class PedidoController : ControllerBase
    {
        private readonly DzContext _context;

        public PedidoController(DzContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = Papel.Admin)]
        public IEnumerable<Pedido> GetPedidos()
        {
            return _context.Pedidos;
        }

        [HttpGet("consumidor/{idConsumidor}")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public IEnumerable<Pedido> GetPedidos(int idConsumidor)
        {
            return _context.Pedidos.Where(x => x.IdConsumidor == idConsumidor);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public async Task<IActionResult> GetPedido([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPost]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public async Task<IActionResult> PostPedido([FromBody] Pedido pedido)
        {
            ConsumidorController consumidorController = new ConsumidorController(_context);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Verifica itens do pedido
            if (pedido.Itens.Count == 0)
            {
                return BadRequest(new { message = "É necessário pelo menos um produto no pedido!" });
            }

            //Verifica saldo do consumidor
            double totalPedido = 0;
            foreach (var item in pedido.Itens)
            {
                totalPedido += item.ValorUnitarioDZ * item.Quantidade;

                //Limpa o produto se vier preenchido, para evitar duplicações
                item.Produto = null;
            }
            if (totalPedido > consumidorController.SaldoConsumidor(pedido.IdConsumidor))
            {
                return BadRequest(new { message = "O saldo do consumidor é insuficiente!" });
            }

            //Corrige os status caso o registro tenha chegado sujo
            pedido.DataSeparacao = pedido.DataEmissaoNF = pedido.DataEnvio = pedido.DataRecebimento = pedido.DataCancelamento = null;
            pedido.Status = StatusPedido.Realizado;

            foreach (var item in pedido.Itens)
            {
                ConsumidorMovimentacao movimentacao = new ConsumidorMovimentacao()
                {
                    IdConsumidor = pedido.IdConsumidor,
                    Data = DateTime.Now,
                    Natureza = Natureza.Debito,
                    Valor = item.ValorUnitarioDZ * item.Quantidade * -1,
                    Pedido = item
                };
                _context.ConsumidorMovimentacoes.Add(movimentacao);
            }

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedido", new { id = pedido.IdPedido }, pedido);
        }

        [HttpPut("cancelar/{id}")]
        [Authorize(Roles = Papel.Consumidor + "," + Papel.Admin)]
        public async Task<IActionResult> CancelarPedido([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pedido pedido = _context.Pedidos.Find(id);
            if (pedido.Status != StatusPedido.Realizado)
            {
                return BadRequest(new { message = "O pedido não pode mais ser cancelado!" });
            }
            pedido.DataCancelamento = DateTime.Now;
            pedido.Status = StatusPedido.Cancelado;

            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        [HttpPut("enviarSeparacao/{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> EnviarSeparacao([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pedido pedido = _context.Pedidos.Find(id);
            if (pedido.Status != StatusPedido.Realizado)
            {
                return BadRequest(new { message = "Status não permite enviar para separação!" });
            }
            pedido.DataSeparacao = DateTime.Now;
            pedido.Status = StatusPedido.EmSeparacao;

            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        [HttpPut("emitirNF/{id}/{numeroNF}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> EmitirNF([FromRoute] int id, [FromRoute] string numeroNF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pedido pedido = _context.Pedidos.Find(id);
            if (pedido.Status != StatusPedido.EmSeparacao)
            {
                return BadRequest(new { message = "Status não permite emitir NF!" });
            }
            pedido.DataEmissaoNF = DateTime.Now;
            pedido.NumeroNF = numeroNF;
            pedido.Status = StatusPedido.NotaEmitida;

            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        [HttpPut("enviar/{id}/{codRastreio}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> Enviar([FromRoute] int id, [FromRoute] string codRastreio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pedido pedido = _context.Pedidos.Find(id);
            if (pedido.Status != StatusPedido.NotaEmitida)
            {
                return BadRequest(new { message = "Status não permite envio!" });
            }
            pedido.DataEnvio = DateTime.Now;
            pedido.CodigoRastreio = codRastreio;
            pedido.Status = StatusPedido.Enviado;

            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        [HttpPut("confirmarRecebimento/{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> ConfirmarRecebimento([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pedido pedido = _context.Pedidos.Find(id);
            if (pedido.Status != StatusPedido.Enviado)
            {
                return BadRequest(new { message = "Status não permite confirmar recebimento!" });
            }
            pedido.DataRecebimento = DateTime.Now;
            pedido.Status = StatusPedido.Recebido;

            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }
    }
}