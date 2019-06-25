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
    public class ProdutoSubcategoriaController : ControllerBase
    {
        private readonly DzContext _context;

        public ProdutoSubcategoriaController(DzContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = Papel.Admin)]
        public IEnumerable<ProdutoSubcategoria> GetProdutoSubcategorias()
        {
            return _context.ProdutoSubcategorias;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> GetProdutoSubcategoria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produtoSubcategoria = await _context.ProdutoSubcategorias.FindAsync(id);

            if (produtoSubcategoria == null)
            {
                return NotFound();
            }

            return Ok(produtoSubcategoria);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> PutProdutoSubcategoria([FromRoute] int id, [FromBody] ProdutoSubcategoria produtoSubcategoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != produtoSubcategoria.IdSubcategoria)
            {
                return BadRequest();
            }

            if (_context.ProdutoSubcategorias.Count(x => x.Nome == produtoSubcategoria.Nome && x.IdCategoria == produtoSubcategoria.IdCategoria && x.IdSubcategoria != produtoSubcategoria.IdSubcategoria) > 0)
            {
                return BadRequest(new { message = "Já existe subcategoria com esse nome!" });
            }

            _context.Entry(produtoSubcategoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoSubcategoriaExists(id))
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
        public async Task<IActionResult> PostProdutoSubcategoria([FromBody] ProdutoSubcategoria produtoSubcategoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.ProdutoSubcategorias.Count(x => x.Nome == produtoSubcategoria.Nome && x.IdCategoria == produtoSubcategoria.IdCategoria) > 0)
            {
                return BadRequest(new { message = "Já existe subcategoria com esse nome!" });
            }

            _context.ProdutoSubcategorias.Add(produtoSubcategoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProdutoSubcategoria", new { id = produtoSubcategoria.IdSubcategoria }, produtoSubcategoria);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Papel.Admin)]
        public async Task<IActionResult> DeleteProdutoSubcategoria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produtoSubcategoria = await _context.ProdutoSubcategorias.FindAsync(id);
            if (produtoSubcategoria == null)
            {
                return NotFound();
            }

            _context.ProdutoSubcategorias.Remove(produtoSubcategoria);
            await _context.SaveChangesAsync();

            return Ok(produtoSubcategoria);
        }

        private bool ProdutoSubcategoriaExists(int id)
        {
            return _context.ProdutoSubcategorias.Any(e => e.IdSubcategoria == id);
        }
    }
}