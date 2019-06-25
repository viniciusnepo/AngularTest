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
    public class ProdutoCategoriaController : ControllerBase
    {
        private readonly DzContext _context;

        public ProdutoCategoriaController(DzContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = Papel.Admin)]
        public IEnumerable<ProdutoCategoria> GetProdutoCategorias()
        {
            return _context.ProdutoCategorias;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> GetProdutoCategoria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produtoCategoria = await _context.ProdutoCategorias.FindAsync(id);

            if (produtoCategoria == null)
            {
                return NotFound();
            }

            return Ok(produtoCategoria);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> PutProdutoCategoria([FromRoute] int id, [FromBody] ProdutoCategoria produtoCategoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != produtoCategoria.IdCategoria)
            {
                return BadRequest();
            }

            if (_context.ProdutoCategorias.Count(x => x.Nome == produtoCategoria.Nome && x.IdCategoria != produtoCategoria.IdCategoria) > 0)
            {
                return BadRequest(new { message = "Já existe categoria com esse nome!" });
            }

            _context.Entry(produtoCategoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoCategoriaExists(id))
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
        public async Task<IActionResult> PostProdutoCategoria([FromBody] ProdutoCategoria produtoCategoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.ProdutoCategorias.Count(x => x.Nome == produtoCategoria.Nome) > 0)
            {
                return BadRequest(new { message = "Já existe categoria com esse nome!" });
            }

            _context.ProdutoCategorias.Add(produtoCategoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProdutoCategoria", new { id = produtoCategoria.IdCategoria }, produtoCategoria);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> DeleteProdutoCategoria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produtoCategoria = await _context.ProdutoCategorias.FindAsync(id);
            if (produtoCategoria == null)
            {
                return NotFound();
            }

            _context.ProdutoCategorias.Remove(produtoCategoria);
            await _context.SaveChangesAsync();

            return Ok(produtoCategoria);
        }

        private bool ProdutoCategoriaExists(int id)
        {
            return _context.ProdutoCategorias.Any(e => e.IdCategoria == id);
        }
    }
}