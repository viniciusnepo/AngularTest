using Microsoft.EntityFrameworkCore.Migrations;

namespace Dotz.Api.Migrations
{
    public partial class AlteracaoPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValorDZ",
                table: "PedidoItem",
                newName: "ValorUnitarioDZ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValorUnitarioDZ",
                table: "PedidoItem",
                newName: "ValorDZ");
        }
    }
}
