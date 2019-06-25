using Microsoft.EntityFrameworkCore.Migrations;

namespace Dotz.Api.Migrations
{
    public partial class AlteracaoPedido1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoRastreio",
                table: "Pedidos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroNF",
                table: "Pedidos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoRastreio",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "NumeroNF",
                table: "Pedidos");
        }
    }
}
