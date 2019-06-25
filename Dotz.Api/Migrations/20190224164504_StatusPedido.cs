using Microsoft.EntityFrameworkCore.Migrations;

namespace Dotz.Api.Migrations
{
    public partial class StatusPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumidorMovimentacao_Consumidores_IdConsumidor",
                table: "ConsumidorMovimentacao");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsumidorMovimentacao_ParceiroCreditos_IdParceiroCredito",
                table: "ConsumidorMovimentacao");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsumidorMovimentacao_PedidoItem_IdPedidoItem",
                table: "ConsumidorMovimentacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsumidorMovimentacao",
                table: "ConsumidorMovimentacao");

            migrationBuilder.RenameTable(
                name: "ConsumidorMovimentacao",
                newName: "ConsumidorMovimentacoes");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumidorMovimentacao_IdPedidoItem",
                table: "ConsumidorMovimentacoes",
                newName: "IX_ConsumidorMovimentacoes_IdPedidoItem");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumidorMovimentacao_IdParceiroCredito",
                table: "ConsumidorMovimentacoes",
                newName: "IX_ConsumidorMovimentacoes_IdParceiroCredito");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumidorMovimentacao_IdConsumidor",
                table: "ConsumidorMovimentacoes",
                newName: "IX_ConsumidorMovimentacoes_IdConsumidor");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Pedidos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsumidorMovimentacoes",
                table: "ConsumidorMovimentacoes",
                column: "IdConsumidorMovimentacao");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumidorMovimentacoes_Consumidores_IdConsumidor",
                table: "ConsumidorMovimentacoes",
                column: "IdConsumidor",
                principalTable: "Consumidores",
                principalColumn: "IdConsumidor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumidorMovimentacoes_ParceiroCreditos_IdParceiroCredito",
                table: "ConsumidorMovimentacoes",
                column: "IdParceiroCredito",
                principalTable: "ParceiroCreditos",
                principalColumn: "IdParceiroCredito",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumidorMovimentacoes_PedidoItem_IdPedidoItem",
                table: "ConsumidorMovimentacoes",
                column: "IdPedidoItem",
                principalTable: "PedidoItem",
                principalColumn: "IdPedidoItem",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumidorMovimentacoes_Consumidores_IdConsumidor",
                table: "ConsumidorMovimentacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsumidorMovimentacoes_ParceiroCreditos_IdParceiroCredito",
                table: "ConsumidorMovimentacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsumidorMovimentacoes_PedidoItem_IdPedidoItem",
                table: "ConsumidorMovimentacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsumidorMovimentacoes",
                table: "ConsumidorMovimentacoes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Pedidos");

            migrationBuilder.RenameTable(
                name: "ConsumidorMovimentacoes",
                newName: "ConsumidorMovimentacao");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumidorMovimentacoes_IdPedidoItem",
                table: "ConsumidorMovimentacao",
                newName: "IX_ConsumidorMovimentacao_IdPedidoItem");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumidorMovimentacoes_IdParceiroCredito",
                table: "ConsumidorMovimentacao",
                newName: "IX_ConsumidorMovimentacao_IdParceiroCredito");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumidorMovimentacoes_IdConsumidor",
                table: "ConsumidorMovimentacao",
                newName: "IX_ConsumidorMovimentacao_IdConsumidor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsumidorMovimentacao",
                table: "ConsumidorMovimentacao",
                column: "IdConsumidorMovimentacao");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumidorMovimentacao_Consumidores_IdConsumidor",
                table: "ConsumidorMovimentacao",
                column: "IdConsumidor",
                principalTable: "Consumidores",
                principalColumn: "IdConsumidor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumidorMovimentacao_ParceiroCreditos_IdParceiroCredito",
                table: "ConsumidorMovimentacao",
                column: "IdParceiroCredito",
                principalTable: "ParceiroCreditos",
                principalColumn: "IdParceiroCredito",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumidorMovimentacao_PedidoItem_IdPedidoItem",
                table: "ConsumidorMovimentacao",
                column: "IdPedidoItem",
                principalTable: "PedidoItem",
                principalColumn: "IdPedidoItem",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
