using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dotz.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consumidores",
                columns: table => new
                {
                    IdConsumidor = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    Senha = table.Column<string>(nullable: false),
                    CPF = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    Sobrenome = table.Column<string>(nullable: false),
                    Sexo = table.Column<int>(nullable: false),
                    DtNascimento = table.Column<DateTime>(nullable: false),
                    TelefoneResidencial = table.Column<string>(nullable: true),
                    TelefoneCelular = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumidores", x => x.IdConsumidor);
                });

            migrationBuilder.CreateTable(
                name: "Parceiros",
                columns: table => new
                {
                    IdParceiro = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    Senha = table.Column<string>(nullable: false),
                    CNPJ = table.Column<string>(nullable: false),
                    RazaoSocial = table.Column<string>(nullable: false),
                    NomeFantasia = table.Column<string>(nullable: false),
                    LogoURL = table.Column<string>(nullable: true),
                    Regulamento = table.Column<string>(nullable: false),
                    FatorDZ = table.Column<int>(nullable: false),
                    MoedaTroca = table.Column<int>(nullable: false),
                    URLRedirecionamento = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parceiros", x => x.IdParceiro);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoCategorias",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoCategorias", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    Senha = table.Column<string>(nullable: true),
                    Papel = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsumidorEndereco",
                columns: table => new
                {
                    IdConsumidorEndereco = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdConsumidor = table.Column<int>(nullable: false),
                    CEP = table.Column<string>(maxLength: 8, nullable: false),
                    Logradouro = table.Column<string>(nullable: false),
                    Numero = table.Column<string>(maxLength: 20, nullable: false),
                    Complemento = table.Column<string>(nullable: true),
                    Bairro = table.Column<string>(maxLength: 20, nullable: false),
                    Cidade = table.Column<string>(maxLength: 20, nullable: false),
                    UF = table.Column<string>(maxLength: 2, nullable: false),
                    Principal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumidorEndereco", x => x.IdConsumidorEndereco);
                    table.ForeignKey(
                        name: "FK_ConsumidorEndereco_Consumidores_IdConsumidor",
                        column: x => x.IdConsumidor,
                        principalTable: "Consumidores",
                        principalColumn: "IdConsumidor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdConsumidor = table.Column<int>(nullable: false),
                    DataPedido = table.Column<DateTime>(nullable: false),
                    DataSeparacao = table.Column<DateTime>(nullable: true),
                    DataEmissaoNF = table.Column<DateTime>(nullable: true),
                    DataEnvio = table.Column<DateTime>(nullable: true),
                    DataRecebimento = table.Column<DateTime>(nullable: true),
                    DataCancelamento = table.Column<DateTime>(nullable: true),
                    Observacoes = table.Column<string>(nullable: true),
                    CEP = table.Column<string>(maxLength: 8, nullable: false),
                    Logradouro = table.Column<string>(nullable: false),
                    Numero = table.Column<string>(maxLength: 20, nullable: false),
                    Complemento = table.Column<string>(nullable: true),
                    Bairro = table.Column<string>(maxLength: 20, nullable: false),
                    Cidade = table.Column<string>(maxLength: 20, nullable: false),
                    UF = table.Column<string>(maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Consumidores_IdConsumidor",
                        column: x => x.IdConsumidor,
                        principalTable: "Consumidores",
                        principalColumn: "IdConsumidor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParceiroCreditos",
                columns: table => new
                {
                    IdParceiroCredito = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdParceiro = table.Column<int>(nullable: false),
                    IdConsumidor = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    ValorDZ = table.Column<double>(nullable: false),
                    ValorMoedaOriginal = table.Column<double>(nullable: true),
                    MoedaOriginal = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParceiroCreditos", x => x.IdParceiroCredito);
                    table.ForeignKey(
                        name: "FK_ParceiroCreditos_Consumidores_IdConsumidor",
                        column: x => x.IdConsumidor,
                        principalTable: "Consumidores",
                        principalColumn: "IdConsumidor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParceiroCreditos_Parceiros_IdParceiro",
                        column: x => x.IdParceiro,
                        principalTable: "Parceiros",
                        principalColumn: "IdParceiro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoSubcategorias",
                columns: table => new
                {
                    IdSubcategoria = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCategoria = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoSubcategorias", x => x.IdSubcategoria);
                    table.ForeignKey(
                        name: "FK_ProdutoSubcategorias_ProdutoCategorias_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "ProdutoCategorias",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    IdProduto = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdSubcategoria = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    Descricao = table.Column<string>(nullable: false),
                    Marca = table.Column<string>(nullable: false),
                    EAN = table.Column<string>(nullable: false),
                    QuantidadeEstoque = table.Column<int>(nullable: false),
                    ValorDotz = table.Column<double>(nullable: false),
                    ValorDotzPromocional = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.IdProduto);
                    table.ForeignKey(
                        name: "FK_Produtos_ProdutoSubcategorias_IdSubcategoria",
                        column: x => x.IdSubcategoria,
                        principalTable: "ProdutoSubcategorias",
                        principalColumn: "IdSubcategoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoItem",
                columns: table => new
                {
                    IdPedidoItem = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPedido = table.Column<int>(nullable: false),
                    IdProduto = table.Column<int>(nullable: false),
                    Quantidade = table.Column<int>(nullable: false),
                    ValorDZ = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItem", x => x.IdPedidoItem);
                    table.ForeignKey(
                        name: "FK_PedidoItem_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItem_Produtos_IdProduto",
                        column: x => x.IdProduto,
                        principalTable: "Produtos",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoImagem",
                columns: table => new
                {
                    IdProdutoImagem = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdProduto = table.Column<int>(nullable: false),
                    URL = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoImagem", x => x.IdProdutoImagem);
                    table.ForeignKey(
                        name: "FK_ProdutoImagem_Produtos_IdProduto",
                        column: x => x.IdProduto,
                        principalTable: "Produtos",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsumidorMovimentacao",
                columns: table => new
                {
                    IdConsumidorMovimentacao = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdConsumidor = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Natureza = table.Column<int>(nullable: false),
                    Valor = table.Column<double>(nullable: false),
                    IdPedidoItem = table.Column<int>(nullable: true),
                    IdParceiroCredito = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumidorMovimentacao", x => x.IdConsumidorMovimentacao);
                    table.ForeignKey(
                        name: "FK_ConsumidorMovimentacao_Consumidores_IdConsumidor",
                        column: x => x.IdConsumidor,
                        principalTable: "Consumidores",
                        principalColumn: "IdConsumidor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsumidorMovimentacao_ParceiroCreditos_IdParceiroCredito",
                        column: x => x.IdParceiroCredito,
                        principalTable: "ParceiroCreditos",
                        principalColumn: "IdParceiroCredito",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumidorMovimentacao_PedidoItem_IdPedidoItem",
                        column: x => x.IdPedidoItem,
                        principalTable: "PedidoItem",
                        principalColumn: "IdPedidoItem",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumidorEndereco_IdConsumidor",
                table: "ConsumidorEndereco",
                column: "IdConsumidor");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumidorMovimentacao_IdConsumidor",
                table: "ConsumidorMovimentacao",
                column: "IdConsumidor");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumidorMovimentacao_IdParceiroCredito",
                table: "ConsumidorMovimentacao",
                column: "IdParceiroCredito");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumidorMovimentacao_IdPedidoItem",
                table: "ConsumidorMovimentacao",
                column: "IdPedidoItem");

            migrationBuilder.CreateIndex(
                name: "IX_ParceiroCreditos_IdConsumidor",
                table: "ParceiroCreditos",
                column: "IdConsumidor");

            migrationBuilder.CreateIndex(
                name: "IX_ParceiroCreditos_IdParceiro",
                table: "ParceiroCreditos",
                column: "IdParceiro");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_IdPedido",
                table: "PedidoItem",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_IdProduto",
                table: "PedidoItem",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdConsumidor",
                table: "Pedidos",
                column: "IdConsumidor");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoImagem_IdProduto",
                table: "ProdutoImagem",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_IdSubcategoria",
                table: "Produtos",
                column: "IdSubcategoria");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoSubcategorias_IdCategoria",
                table: "ProdutoSubcategorias",
                column: "IdCategoria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumidorEndereco");

            migrationBuilder.DropTable(
                name: "ConsumidorMovimentacao");

            migrationBuilder.DropTable(
                name: "ProdutoImagem");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "ParceiroCreditos");

            migrationBuilder.DropTable(
                name: "PedidoItem");

            migrationBuilder.DropTable(
                name: "Parceiros");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Consumidores");

            migrationBuilder.DropTable(
                name: "ProdutoSubcategorias");

            migrationBuilder.DropTable(
                name: "ProdutoCategorias");
        }
    }
}
