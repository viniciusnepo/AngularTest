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
    public class ProdutoController : ControllerBase
    {
        private readonly DzContext _context;

        public ProdutoController(DzContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = Papel.Admin)]
        public IEnumerable<Produto> GetProdutos()
        {
            return _context.Produtos;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> GetProduto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produto = await _context.Produtos.Include(x => x.Imagens).FirstOrDefaultAsync(x => x.IdProduto == id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> PutProduto([FromRoute] int id, [FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != produto.IdProduto)
            {
                return BadRequest();
            }

            if (_context.Produtos.Count(x => x.EAN == produto.EAN && x.IdProduto != id) > 0)
            {
                return BadRequest(new { message = "Já existe produto com esse código EAN!" });
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
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
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> PostProduto([FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Produtos.Count(x => x.EAN == produto.EAN) > 0)
            {
                return BadRequest(new { message = "Já existe produto com esse código EAN!" });
            }

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduto", new { id = produto.IdProduto }, produto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> DeleteProduto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.IdProduto == id);
        }

        [HttpGet("imagens/{idProduto}")]
        [Authorize(Roles = Papel.Admin)]
        public IEnumerable<ProdutoImagem> GetProdutoImagem([FromRoute] int idProduto)
        {
            return _context.ProdutoImagem.Where(x => x.IdProduto == idProduto);
        }

        [HttpPost("imagens")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> PostProdutoImagem([FromBody] ProdutoImagem produtoImagem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ProdutoImagem.Add(produtoImagem);
            await _context.SaveChangesAsync();

            return Ok(produtoImagem);
        }

        [HttpDelete("imagens/{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> DeleteProdutoImagem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produtoImagem = await _context.ProdutoImagem.FindAsync(id);
            if (produtoImagem == null)
            {
                return NotFound();
            }

            _context.ProdutoImagem.Remove(produtoImagem);
            await _context.SaveChangesAsync();

            return Ok(produtoImagem);
        }
    }
}