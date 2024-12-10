using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounts.Core.Migrations
{
    public partial class AddReportPurchases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmountReceived_SalesDetails_SalesDetailsId",
                table: "AmountReceived");

            migrationBuilder.DropIndex(
                name: "IX_AmountReceived_SalesDetailsId",
                table: "AmountReceived");

            migrationBuilder.DropColumn(
                name: "SalesDetailsId",
                table: "AmountReceived");

            migrationBuilder.CreateTable(
                name: "PurchaseReports",
                columns: table => new
                {
                    PurchaseSlipNo = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PartyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalItems = table.Column<long>(type: "bigint", nullable: false),
                    BillAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmountReceived_SalesMasterId",
                table: "AmountReceived",
                column: "SalesMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AmountReceived_SalesMasters_SalesMasterId",
                table: "AmountReceived",
                column: "SalesMasterId",
                principalTable: "SalesMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmountReceived_SalesMasters_SalesMasterId",
                table: "AmountReceived");

            migrationBuilder.DropTable(
                name: "PurchaseReports");

            migrationBuilder.DropIndex(
                name: "IX_AmountReceived_SalesMasterId",
                table: "AmountReceived");

            migrationBuilder.AddColumn<long>(
                name: "SalesDetailsId",
                table: "AmountReceived",
                type: "bigint",
                nullable: true);

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
        }
    }
}
