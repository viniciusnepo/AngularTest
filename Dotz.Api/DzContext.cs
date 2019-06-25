using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotz.Api;

namespace Dotz.Api
{
    public class DzContext : DbContext
    {
        public DzContext(DbContextOptions<DzContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Consumidor> Consumidores { get; set; }
        public DbSet<ConsumidorEndereco> ConsumidorEnderecos { get; set; }
        public DbSet<ConsumidorMovimentacao> ConsumidorMovimentacoes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoCategoria> ProdutoCategorias { get; set; }
        public DbSet<ProdutoSubcategoria> ProdutoSubcategorias { get; set; }
        public DbSet<Parceiro> Parceiros { get; set; }
        public DbSet<ParceiroCredito> ParceiroCreditos { get; set; }
        public DbSet<Dotz.Api.ProdutoImagem> ProdutoImagem { get; set; }
    }
}
