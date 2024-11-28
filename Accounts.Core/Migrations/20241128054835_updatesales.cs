using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class updatesales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesDetails_SalesMasters_SalesMasterId",
                table: "SalesDetails");

            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "PurchaseDetails",
                newName: "ItemId");

            migrationBuilder.AlterColumn<long>(
                name: "SalesMasterId",
                table: "SalesDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SalesMasterId",
                table: "AmountReceived",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesDetails_SalesMasters_SalesMasterId",
                table: "SalesDetails",
                column: "SalesMasterId",
                principalTable: "SalesMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesDetails_SalesMasters_SalesMasterId",
                table: "SalesDetails");

            migrationBuilder.DropColumn(
                name: "SalesMasterId",
                table: "AmountReceived");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "PurchaseDetails",
                newName: "StockId");

            migrationBuilder.AlterColumn<long>(
                name: "SalesMasterId",
                table: "SalesDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesDetails_SalesMasters_SalesMasterId",
                table: "SalesDetails",
                column: "SalesMasterId",
                principalTable: "SalesMasters",
                principalColumn: "Id");
        }
    }
}
