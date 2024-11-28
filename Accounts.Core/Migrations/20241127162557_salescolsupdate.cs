using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class salescolsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesDetails_AmountReceived_AmountReceivedId",
                table: "SalesDetails");

            migrationBuilder.DropIndex(
                name: "IX_SalesDetails_AmountReceivedId",
                table: "SalesDetails");

            migrationBuilder.DropColumn(
                name: "AmountReceivedId",
                table: "SalesDetails");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "ItemMaster");

            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "SalesDetails",
                newName: "ItemId");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "SalesMasters",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "SalesMasterId",
                table: "SalesDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "SalesDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "SalesDetailsId",
                table: "AmountReceived",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetails_SalesMasterId",
                table: "SalesDetails",
                column: "SalesMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_AmountReceived_SalesDetailsId",
                table: "AmountReceived",
                column: "SalesDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AmountReceived_SalesDetails_SalesDetailsId",
                table: "AmountReceived",
                column: "SalesDetailsId",
                principalTable: "SalesDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesDetails_SalesMasters_SalesMasterId",
                table: "SalesDetails",
                column: "SalesMasterId",
                principalTable: "SalesMasters",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmountReceived_SalesDetails_SalesDetailsId",
                table: "AmountReceived");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesDetails_SalesMasters_SalesMasterId",
                table: "SalesDetails");

            migrationBuilder.DropIndex(
                name: "IX_SalesDetails_SalesMasterId",
                table: "SalesDetails");

            migrationBuilder.DropIndex(
                name: "IX_AmountReceived_SalesDetailsId",
                table: "AmountReceived");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "SalesMasters");

            migrationBuilder.DropColumn(
                name: "SalesMasterId",
                table: "SalesDetails");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "SalesDetails");

            migrationBuilder.DropColumn(
                name: "SalesDetailsId",
                table: "AmountReceived");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "SalesDetails",
                newName: "StockId");

            migrationBuilder.AddColumn<long>(
                name: "AmountReceivedId",
                table: "SalesDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ItemId",
                table: "ItemMaster",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SalesDetails_AmountReceivedId",
                table: "SalesDetails",
                column: "AmountReceivedId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesDetails_AmountReceived_AmountReceivedId",
                table: "SalesDetails",
                column: "AmountReceivedId",
                principalTable: "AmountReceived",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
